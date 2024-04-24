using System.Text.RegularExpressions;

namespace WebAPISample.Services
{
    public interface ICheckService
    {
        bool ChkColumnValue(string strColValue, CheckMode checkMode);
    }
    public class CheckService : ICheckService
    {
        public bool ChkColumnValue(string strColValue, CheckMode checkMode)
        {
            switch (checkMode)
            {
                case CheckMode.chkSpecialSign://檢核是否含有無效的字元
                    return !Regex.IsMatch(strColValue, @"[()#'\""""\-\*\<\>\/\\\%\=\&\+\?\;]+");

                case CheckMode.chkOnlyNumAndSpecialSign://檢核是否只有數字與#,$,@,!
                    return Regex.IsMatch(strColValue, @"^[0-9#$@!]*$");

                case CheckMode.chkOnlyNum://檢核是否只含有數字
                    return Regex.IsMatch(strColValue, @"^[\d]*$");

                case CheckMode.chkOnlyNumAndEng://檢核是否只含有數字及英文
                    return Regex.IsMatch(strColValue, @"^[a-zA-Z0-9]*$");

                case CheckMode.chkEmail://檢核Email
                    return Regex.IsMatch(strColValue, @"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$");

                case CheckMode.chkDate://檢核日期,合法格式包含:20221221,2022/12/21,2022-12-21,2022 12 21(需有8位數)
                    strColValue = strColValue.Trim().Replace("/", "").Replace(" ", "").Replace("-", "");
                    if (strColValue.Length != 8) return false;
                    return DateTime.TryParse($"{strColValue.Substring(0, 4)}/{strColValue.Substring(4, 2)}/{strColValue.Substring(6, 2)}", out DateTime dt);

                case CheckMode.chkURL://檢核URL
                    return !Regex.IsMatch(strColValue, @"[()#'\""""\*\<\>\]+\[\|\^\@\!\~\`\$\%]");

                case CheckMode.chkIP://檢核IP,格式為4~6組1-3位數字組成,每組數字需小於256(如110.52.4.28)
                    string[] arrIP = strColValue.Split('.');
                    if (arrIP.Length == 4 || arrIP.Length == 6)
                    {
                        for (int i = 0; i < arrIP.Length; i++)
                        {
                            if (!int.TryParse(arrIP[i].Trim(), out int iIP)) return false;
                            if (iIP > 255) return false;
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case CheckMode.chkPwd://檢核密碼
                    return Regex.IsMatch(strColValue, @"^[a-zA-Z0-9_!@#$\-\%\^\&]*$");

                case CheckMode.chkEmpty://檢核值是否為空
                    return strColValue.Trim() != "";

                case CheckMode.chkForder://檢核FTP連線資料夾
                    return Regex.IsMatch(strColValue, @"^[a-zA-Z0-9_\\\\/\-\(\)]*$");
                default:
                    return false;
            }
        }
    }
    public enum CheckMode
    {
        chkSpecialSign = 0,
        chkOnlyNumAndSpecialSign = 1,
        chkOnlyNum = 2,
        chkOnlyNumAndEng = 3,
        chkEmail = 4,
        chkDate = 5,
        chkURL = 6,
        chkIP = 7,
        chkPwd = 8,
        chkEmpty = 9,
        chkForder = 10,
    }
}
