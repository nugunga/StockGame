using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScrollCarousel
{
    public class Carousel : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [Header("Items")] [Description("List of items to be displayed in the carousel")]
        public List<RectTransform> Items = new();

        [Header("Position")] [Description("Index of the item that will be centered at the start")]
        public int StartItem;

        [Description("Spacing between items")] public float Itemspacing = 50f;

        [Header("Scale")] [Description("Scale of the centered item")]
        public float CenteredScale = 1f;

        [Description("Scale of the non-centered items")]
        public float NonCenteredScale = 0.7f;

        [Header("Rotation")] [Description("Maximum rotation angle of the items")] [SerializeField]
        public float MaxRotationAngle = 10f;

        [SerializeField] private float _rotationSmoothSpeed = 5f;

        [Header("Swipe Settings")] [SerializeField]
        private float _snapSpeed = 10f;

        [Description("Enable infinite scrolling")] [SerializeField]
        public bool InfiniteScroll;

        [Description("Radius of the infinite scroll circle")] [SerializeField]
        public float CircleRadius = 500f;

        [Header("Colors")] [Description("Enable color animation")]
        public bool ColorAnimation;

        [Description("Color of the focused item")]
        public Color FocustedColor = Color.white;

        [Description("Color of the non-focused items")]
        public Color NonFocustedColor = Color.gray;

        private readonly Dictionary<RectTransform, Coroutine> _activeColorAnimations = new();
        private int _currentItemIndex;
        private float _currentRotationOffset;
        private bool _isSnapping;

        private RectTransform _rectTransform;
        private Vector2 _startDragPosition;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            FocusItem(StartItem);
            ForceUpdate();
        }

        private void Update()
        {
            if (_isSnapping) MoveToItem();

            UpdateItemsAppearance();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isSnapping = false;
            _startDragPosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Items.Count == 0) return;

            if (InfiniteScroll)
            {
                var rotationDelta = eventData.delta.x / CircleRadius * 45f;
                _currentRotationOffset += rotationDelta;
                RotateItemsCircular(_currentRotationOffset);
            }
            else
            {
                var leftBound = _rectTransform.rect.center.x + GetTotalOffset(0);
                var rightBound = _rectTransform.rect.center.x + GetTotalOffset(Items.Count - 1);

                var currentCenterItemPos = Items[_currentItemIndex].anchoredPosition.x;

                if ((currentCenterItemPos >= leftBound && eventData.delta.x > 0) ||
                    (currentCenterItemPos <= rightBound && eventData.delta.x < 0))
                {
                    var dragFactor = 1f;
                    if (currentCenterItemPos > leftBound ||
                        currentCenterItemPos < rightBound)
                        dragFactor = 0.5f;

                    foreach (var item in Items) item.anchoredPosition += new Vector2(eventData.delta.x * dragFactor, 0);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (InfiniteScroll)
            {
                var closestDistance = float.MaxValue;
                var closestIndex = 0;
                for (var i = 0; i < Items.Count; i++)
                {
                    var angle = 360f / Items.Count * (i - _currentItemIndex) + _currentRotationOffset;
                    var distance = Mathf.Abs(Mathf.DeltaAngle(0, angle));
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestIndex = i;
                    }
                }

                FocusItem(closestIndex);
            }
            else
            {
                var centerX = _rectTransform.rect.center.x;
                var closestIndex = 0;
                var closestDistance = float.MaxValue;
                for (var i = 0; i < Items.Count; i++)
                {
                    var distance = Mathf.Abs(Items[i].anchoredPosition.x - centerX);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestIndex = i;
                    }
                }

                FocusItem(closestIndex);
            }

            _currentRotationOffset = 0f;
            _startDragPosition = Vector2.zero;
        }

        private float GetItemspacing(int index)
        {
            var currentItemscale = index == _currentItemIndex ? CenteredScale : NonCenteredScale;
            var nextItemscale = index + 1 == _currentItemIndex ? CenteredScale : NonCenteredScale;

            var currentWidth = Items[index].rect.width * currentItemscale;
            var nextWidth = Items[index + 1].rect.width * nextItemscale;

            return (currentWidth + nextWidth) / 2 + Itemspacing;
        }

        private float GetTotalOffset(int index)
        {
            var offset = 0f;
            var startIdx = Math.Min(index, _currentItemIndex);
            var endIdx = Math.Max(index, _currentItemIndex);

            for (var i = startIdx; i < endIdx; i++) offset += GetItemspacing(i);

            return index < _currentItemIndex ? -offset : offset;
        }

        private void PositionItems(bool animate = true)
        {
            if (Items.Count == 0) return;

            var centerPoint = _rectTransform.rect.center;
            var targetTime = animate ? Time.deltaTime * _snapSpeed : 1f;

            for (var i = 0; i < Items.Count; i++)
            {
                Vector2 targetPosition;
                if (InfiniteScroll)
                {
                    var angle = 360f / Items.Count * (i - _currentItemIndex);
                    var radians = angle * Mathf.Deg2Rad;
                    targetPosition = new Vector2(
                        centerPoint.x + Mathf.Sin(radians) * CircleRadius,
                        centerPoint.y + (1 - Mathf.Cos(radians)) * CircleRadius * 0.5f
                    );
                }
                else
                {
                    var offset = GetTotalOffset(i);
                    targetPosition = new Vector2(centerPoint.x + offset, centerPoint.y);
                }

                if (animate)
                    Items[i].anchoredPosition = Vector2.Lerp(
                        Items[i].anchoredPosition,
                        targetPosition,
                        targetTime
                    );
                else
                    Items[i].anchoredPosition = targetPosition;
            }
        }

        private void UpdateItemsAppearance()
        {
            if (Items.Count == 0) return;

            var centerPoint = _rectTransform.rect.center;
            var maxDistance = InfiniteScroll ? CircleRadius : GetItemspacing(0);
            var minDistance = float.MaxValue;
            var closestIndex = -1;

            for (var i = 0; i < Items.Count; i++)
            {
                if (!Items[i]) continue;

                float distance;
                float angleDistance;
                if (InfiniteScroll)
                {
                    var angle = 360f / Items.Count * (i - _currentItemIndex) + _currentRotationOffset;
                    distance = Mathf.Abs(Mathf.DeltaAngle(0, angle)) / (360f / Items.Count) * CircleRadius;
                    angleDistance = Mathf.Abs(Mathf.DeltaAngle(0, angle)) / (360f / Items.Count);
                }
                else
                {
                    distance = Mathf.Abs(Items[i].anchoredPosition.x - centerPoint.x);
                    angleDistance = Mathf.Abs(i - _currentItemIndex);
                }

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestIndex = i;
                }

                Items[i].SetSiblingIndex(Items.Count - (int)(angleDistance * 2));

                var normalizedDistance = Mathf.Clamp01(distance / maxDistance);

                // Scale
                var targetScale = Mathf.Lerp(CenteredScale, NonCenteredScale, normalizedDistance);
                var newScale = new Vector3(targetScale, targetScale, 1f);
                if (!float.IsNaN(newScale.x) && !float.IsNaN(newScale.y)) Items[i].localScale = newScale;

                // Rotation
                var rotationSign = Items[i].anchoredPosition.x > centerPoint.x ? 1f : -1f;
                var targetRotationY = MaxRotationAngle * normalizedDistance * rotationSign;
                if (!float.IsNaN(targetRotationY))
                    Items[i].localRotation = Quaternion.Slerp(
                        Items[i].localRotation,
                        Quaternion.Euler(30, targetRotationY, 0),
                        Time.deltaTime * _rotationSmoothSpeed
                    );
            }

            if (ColorAnimation)
                for (var i = 0; i < Items.Count; i++)
                {
                    var targetColor = i == closestIndex ? FocustedColor : NonFocustedColor;
                    StartColorAnimation(Items[i], targetColor);
                }
        }

        private void RotateItemsCircular(float rotationOffset)
        {
            var centerPoint = _rectTransform.rect.center;

            for (var i = 0; i < Items.Count; i++)
            {
                var baseAngle = 360f / Items.Count * (i - _currentItemIndex);
                var angle = baseAngle + rotationOffset;
                var radians = angle * Mathf.Deg2Rad;

                var targetPosition = new Vector2(
                    centerPoint.x + Mathf.Sin(radians) * CircleRadius,
                    centerPoint.y + (1 - Mathf.Cos(radians)) * CircleRadius * 0.5f
                );

                Items[i].anchoredPosition = targetPosition;
            }
        }

        private void MoveToItem()
        {
            PositionItems();

            // Check if we're close enough to stop snapping
            var targetItem = Items[_currentItemIndex];
            if (Mathf.Abs(targetItem.anchoredPosition.x - _rectTransform.rect.center.x) < 0.1f)
            {
                _isSnapping = false;
                PositionItems(false);
            }
        }

        public void FocusItem(RectTransform item)
        {
            FocusItem(Items.IndexOf(item));
        }

        private void FocusItem(int index)
        {
            if (index < 0 || index >= Items.Count) return;

            _currentItemIndex = index;
            _currentRotationOffset = 0f;
            _isSnapping = true;

            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                var isFocused = i == _currentItemIndex;

                item.GetComponent<CarouselButton>()?.SetFocus(isFocused);
            }
        }

        public void GoToNext()
        {
            if (InfiniteScroll)
                FocusItem((_currentItemIndex + 1) % Items.Count);
            else if (_currentItemIndex < Items.Count - 1) FocusItem(_currentItemIndex + 1);
        }

        public void GoToPrevious()
        {
            if (InfiniteScroll)
                FocusItem((_currentItemIndex - 1 + Items.Count) % Items.Count);
            else if (_currentItemIndex > 0) FocusItem(_currentItemIndex - 1);
        }

        public void ForceUpdate()
        {
            PositionItems(false);
            UpdateItemsAppearance();
        }

        private void StartColorAnimation(RectTransform item, Color targetColor)
        {
            if (_activeColorAnimations.ContainsKey(item))
            {
                StopCoroutine(_activeColorAnimations[item]);
                _activeColorAnimations.Remove(item);
            }

            _activeColorAnimations[item] = StartCoroutine(ColorAnimationCoroutine(item, targetColor));
        }

        private IEnumerator ColorAnimationCoroutine(RectTransform item, Color targetColor)
        {
            var image = item.GetComponent<Image>();
            if (image == null)
            {
                _activeColorAnimations.Remove(item);
                yield break;
            }

            var startColor = image.color;
            var elapsedTime = 0f;
            var duration = 0.2f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                image.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
                yield return null;
            }

            image.color = targetColor;
            _activeColorAnimations.Remove(item);
        }

        public int GetCurrentItemIndex()
        {
            return _currentItemIndex;
        }
    }
}