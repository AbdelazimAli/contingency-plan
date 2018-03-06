using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Db.Persistence.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public IQueryable<MessageViewModel> ReadMessage(int Id, string culture,int companyId)
        {
            var msg = from m in context.Messages
                      where m.CompanyId==companyId
                      select new MessageViewModel
                      {
                          Id=m.Id,
                          Title = m.Title,
                          Sent = m.Sent,
                          FromEmpId = m.FromEmpId,
                          Body = m.Body,
                          CreatedTime = m.CreatedTime,
                          CreatedUser =m.CreatedUser,
                          CompanyId = m.CompanyId
                      };
            return msg;

        }
        public MessageViewModel ReadFormMessage(int id, string culture)
        {
            var query = context.Messages.Where(a => a.Id == id).Select(a=> new { msg=a, Attachments= HrContext.GetAttachments("EmpMessageForm", a.Id),}).FirstOrDefault();
            var mod = new MessageViewModel()
            {
                Id = query.msg.Id,
                All = query.msg.All,
                Body = query.msg.Body,
                CreatedTime = query.msg.CreatedTime,
                IDepts = query.msg.Depts == null ? null : query.msg.Depts.Split(',').Select(int.Parse).ToList(),
                IEmployees = query.msg.Employees == null ? null : query.msg.Employees.Split(',').Select(int.Parse).ToList(),
                IPeopleGroups = query.msg.PeopleGroups == null ? null : query.msg.PeopleGroups.Split(',').Select(int.Parse).ToList(),
                IJobs = query.msg.Jobs == null ? null : query.msg.Jobs.Split(',').Select(int.Parse).ToList(),
                Sent = query.msg.Sent,
                Title = query.msg.Title,
                Attachments =query.Attachments
            };
            return mod;

        }
        public void Send(int Id, int CompanyId)
        {
            var Massges = context.Messages.Where(m => m.Id == Id).FirstOrDefault();
            IQueryable<Assignment> ActiveAssignments;

            if (Massges.All)
            {
                ActiveAssignments = context.Assignments.Where(a => ((a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today) && a.CompanyId == CompanyId));
            }
            else
            {
                List<int> Depts = Massges.Depts != null ? Massges.Depts.Split(',').Select(int.Parse).ToList() : new List<int>();
                List<int> Jobs = Massges.Jobs != null ? Massges.Jobs.Split(',').Select(int.Parse).ToList() : new List<int>();
                List<int> PeopleGroups = Massges.PeopleGroups != null ? Massges.PeopleGroups.Split(',').Select(int.Parse).ToList() : new List<int>();
                List<int> Employees = Massges.Employees != null ? Massges.Employees.Split(',').Select(int.Parse).ToList() : new List<int>();

                ActiveAssignments = context.Assignments.Where(a => ((a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today) && a.CompanyId == CompanyId) && (Depts.Contains(a.DepartmentId) || Jobs.Contains(a.JobId) || PeopleGroups.Contains(a.GroupId.Value) || Employees.Contains(a.EmpId)));
            }


            foreach (var AllAssign in ActiveAssignments.ToList())
            {
                MsgEmployee newMessage = new MsgEmployee()
                {
                    Message = Massges,
                    MessageId = Massges.Id,
                    FromEmpId = Massges.FromEmpId,
                    ToEmp = AllAssign.Employee,
                    ToEmpId = AllAssign.EmpId
                };
                context.MsgEmployees.Add(newMessage);
            }

            if (ActiveAssignments.Count() > 0)
                context.SaveChanges();
        }
        public IQueryable<EmployeeMessagesViewModel> GetEmployeeMessages(string Culture)
        {
            var query = from m in context.MsgEmployees
                        join p in context.People on m.FromEmpId equals p.Id
                        select new EmployeeMessagesViewModel
                        {
                            Id=m.Id,
                            Body=m.Message.Body,
                            FromEmpId=p.Id,
                            FromEmployee= HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, Culture),
                            Read = m.Read,
                            Title=m.Message.Title,
                            ToEmpId = m.ToEmpId
                        };
            return query;
        }
        public MsgEmployee GetEmpMessage(int Id)
        {
            return (from em in context.MsgEmployees
                    where em.Id == Id
                    select em).FirstOrDefault();
        }
        public Message GetMessage(int Id)
        {
            return (from m in context.Messages
                    where m.Id == Id
                    select m).FirstOrDefault();
        }
        public void Add(MsgEmployee msEmp)
        {
            context.MsgEmployees.Add(msEmp);
        }
        public void Attach(MsgEmployee msEmp)
        {
            context.MsgEmployees.Attach(msEmp);
        }

        public DbEntityEntry<MsgEmployee> Entry(MsgEmployee msEmp)
        {
            return Context.Entry(msEmp);
        }
        public void Remove(MsgEmployee msEmp)
        {
            if (Context.Entry(msEmp).State == EntityState.Detached)
            {
                context.MsgEmployees.Attach(msEmp);
            }
            context.MsgEmployees.Remove(msEmp);
        }

        //for navbar
        public IEnumerable ReadEmpMessages(int CompanyId,int empId, string culture)
        {
            var query = (from m in context.MsgEmployees
                         where m.ToEmpId == empId && !m.Read && m.Message.CompanyId == CompanyId
                         join p in context.People on m.FromEmpId equals p.Id
                         select new NavBarItemVM
                         {
                             Id = m.Id,
                             PicUrl = HrContext.GetDoc("EmployeePic", p.Id),
                             Gender = p.Gender,
                             From = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                             Message = m.Message.Title
                         }).Take(5).ToList();

            return query;
        }

    }
}
