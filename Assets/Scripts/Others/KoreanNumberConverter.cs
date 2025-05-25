using System;
using System.Text;

public static class KoreanNumberConverter
{
    // 기본 숫자 (0-9)
    private static readonly string[] BasicNumbers =
    {
        "", "일", "이", "삼", "사", "오", "육", "칠", "팔", "구"
    };

    // 기본 단위 (십, 백, 천)
    private static readonly string[] BasicUnits =
    {
        "", "십", "백", "천"
    };

    // 큰 단위 (만, 억, 조, 경...)
    private static readonly string[] BigUnits =
    {
        "", "만", "억", "조", "경", "해", "자", "양", "구", "간", "정", "재", "극"
    };

    /// <summary>
    ///     정수를 한글로 변환합니다.
    /// </summary>
    /// <param name="number">변환할 숫자</param>
    /// <returns>한글로 표현된 숫자</returns>
    public static string ToKorean(long number)
    {
        if (number == 0) return "영";

        var isNegative = number < 0;
        if (isNegative) number = -number;

        var result = new StringBuilder();

        // 큰 단위별로 처리 (4자리씩 묶어서)
        var unitIndex = 0;
        while (number > 0)
        {
            var currentGroup = number % 10000;
            number /= 10000;

            if (currentGroup > 0)
            {
                var groupText = ConvertGroupToKorean((int)currentGroup);

                // 만 이상의 단위에서 "일"은 생략
                if (unitIndex > 0 && currentGroup == 1) groupText = "";

                if (!string.IsNullOrEmpty(groupText))
                    result.Insert(0, groupText + BigUnits[unitIndex]);
                else if (unitIndex > 0) result.Insert(0, BigUnits[unitIndex]);
            }

            unitIndex++;
        }

        var finalResult = result.ToString();

        // 음수 처리
        if (isNegative) finalResult = "마이너스 " + finalResult;

        return finalResult;
    }

    /// <summary>
    ///     4자리 이하 숫자를 한글로 변환합니다.
    /// </summary>
    private static string ConvertGroupToKorean(int number)
    {
        if (number == 0) return "";

        var result = new StringBuilder();

        for (var i = 3; i >= 0; i--)
        {
            var digit = number / (int)Math.Pow(10, i) % 10;

            if (digit > 0)
            {
                // 십의 자리에서 1은 생략 (예: "십일" not "일십일")
                if (i == 1 && digit == 1 && number >= 10)
                {
                    result.Append(BasicUnits[i]);
                }
                else
                {
                    result.Append(BasicNumbers[digit]);
                    if (i > 0) result.Append(BasicUnits[i]);
                }
            }
        }

        return result.ToString();
    }

    /// <summary>
    ///     소수점을 포함한 실수를 한글로 변환합니다.
    /// </summary>
    /// <param name="number">변환할 실수</param>
    /// <param name="decimalPlaces">소수점 자릿수 (기본값: 2)</param>
    /// <returns>한글로 표현된 숫자</returns>
    public static string ToKorean(double number, int decimalPlaces = 2)
    {
        if (double.IsNaN(number)) return "숫자가 아님";
        if (double.IsPositiveInfinity(number)) return "양의 무한대";
        if (double.IsNegativeInfinity(number)) return "음의 무한대";

        // 정수 부분과 소수 부분 분리
        var integerPart = (long)Math.Truncate(Math.Abs(number));
        var decimalPart = Math.Abs(number) - integerPart;

        var result = new StringBuilder();

        // 음수 처리
        if (number < 0) result.Append("마이너스 ");

        // 정수 부분
        result.Append(ToKorean(integerPart));

        // 소수 부분
        if (decimalPart > 0 && decimalPlaces > 0)
        {
            result.Append(" 점 ");

            // 소수점 이하 자릿수만큼 처리
            var decimalString = decimalPart.ToString($"F{decimalPlaces}").Substring(2);

            foreach (var digit in decimalString)
            {
                var digitValue = digit - '0';
                result.Append(BasicNumbers[digitValue]);
            }
        }

        return result.ToString();
    }

    /// <summary>
    ///     순수 한글 숫자로 변환 (하나, 둘, 셋...)
    /// </summary>
    public static string ToNativeKorean(int number)
    {
        if (number < 0) return "마이너스 " + ToNativeKorean(-number);

        var nativeNumbers = new[]
        {
            "영", "하나", "둘", "셋", "넷", "다섯",
            "여섯", "일곱", "여덟", "아홉", "열"
        };

        if (number <= 10) return nativeNumbers[number];
        if (number < 20) return "열" + nativeNumbers[number - 10];
        if (number < 100)
        {
            var tens = number / 10;
            var ones = number % 10;
            var tensStr = nativeNumbers[tens] + "십";
            return ones > 0 ? tensStr + nativeNumbers[ones] : tensStr;
        }

        // 100 이상은 한자어 숫자 사용
        return ToKorean(number);
    }
}