using System.Collections.Generic;

namespace MoveStopMove.Core.Events
{
    public enum EEventCode
    {
        // 200 Success
        Success = 200,
        BuySuccess = 201,
        EquipSuccess = 202,
        KillSuccess = 203,
        Win = 210,

        // 400 Player Errors
        NotEnoughCoins = 400,
        EquipFailed = 401,

        // 500 System Errors
        UnknownError = 500,
        DataError = 501,
        SaveError = 502,
    }

    public struct NotificationPopUpEvent
    {
        public bool IsSuccess => (int)Code < 400;
        public EEventCode Code { get; }
        public string Message { get; }

        private static readonly Dictionary<EEventCode, string> s_defaultMessages = new()
        {
            [EEventCode.Success]      = "Thao tác thành công.",
            [EEventCode.BuySuccess]   = "Mua vật phẩm thành công.",
            [EEventCode.EquipSuccess] = "Đã trang bị thành công.",
            [EEventCode.KillSuccess]  = "Tiêu diệt kẻ địch thành công.",
            [EEventCode.Win]          = "Chiến thắng!",

            [EEventCode.NotEnoughCoins] = "Không đủ coin.",
            [EEventCode.EquipFailed]    = "Trang bị thất bại.",

            [EEventCode.UnknownError] = "Đã xảy ra lỗi không xác định.",
            [EEventCode.DataError]    = "Lỗi dữ liệu.",
            [EEventCode.SaveError]    = "Lưu dữ liệu thất bại."
        };

        public NotificationPopUpEvent(EEventCode code)
        {
            Code = code;
            if (s_defaultMessages.TryGetValue(code, out string message))
            {
                Message = message;
            }
            else
            {
                Message = "Thông báo không xác định.";
            }
        }
    }
}