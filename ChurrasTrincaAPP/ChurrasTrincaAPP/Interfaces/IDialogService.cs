﻿using System.Threading.Tasks;

namespace ChurrasTrincaAPP.Interfaces
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
        void ShowToast(string message);
    }
}
