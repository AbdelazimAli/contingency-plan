using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;


namespace Interface.Core.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        void Send(int Id, int CompanyId);
        IQueryable<EmployeeMessagesViewModel> GetEmployeeMessages(string Culture);
        MsgEmployee GetEmpMessage(int Id);
        DbEntityEntry<MsgEmployee> Entry(MsgEmployee MsgEmp);
        void Remove(MsgEmployee MsgEmp);
        void Add(MsgEmployee MsgEmp);
        void Attach(MsgEmployee MsgEmp);
        IQueryable<MessageViewModel> ReadMessage(int Id, string culture,int companyId);
        MessageViewModel ReadFormMessage(int id, string culture);
        Message GetMessage(int Id);

        IEnumerable ReadEmpMessages(int CompanyId, int empId, string culture);
    }
}
