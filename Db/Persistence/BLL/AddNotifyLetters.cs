using Interface.Core;
using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Persistence.BLL
{
    public class AddNotifyLetters
    {
        string Language;
        IHrUnitOfWork unitofwork;
        List<NotifyLetter> RecordsList = new List<NotifyLetter>();
        public AddNotifyLetters(IHrUnitOfWork _unitofwork, NotifyLetter _Record, string _Language)
        {
            unitofwork = _unitofwork;
            Language = _Language;
            if (_Record != null)
                RecordsList.Add(_Record);
        }

        public AddNotifyLetters(IHrUnitOfWork _unitofwork, List<NotifyLetter> _RecordsList, string _Language)
        {
            unitofwork = _unitofwork;
            Language = _Language;
            RecordsList = _RecordsList;
        }

        public bool Run(out string Message,string DefaultErrorMessage="")
        {
            bool Result = false;
            if (!string.IsNullOrEmpty(DefaultErrorMessage))
                Message = DefaultErrorMessage;
            else
                Message = MsgUtils.Instance.Trls(Language, "NotifyLetterFailed");

            int AffRows = 0;
            try
            {
                if (RecordsList != null && RecordsList.Count() > 0)
                {
                    unitofwork.NotifyLetterRepository.AddRange(RecordsList);
                    SendEmails();
                    AffRows = unitofwork.Save();
                }

                if (AffRows > 0)
                {
                    Result = true;
                    Message = MsgUtils.Instance.Trls(Language,"NotifyLetterSent" );
                }
            }
            catch
            {
            }

            return Result;
        }

        private int SendEmails()
        {
            try
            {

            }
            catch
            {

            }
            return 0;
        }
    }

}
