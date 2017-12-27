using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
   public interface IPositionRepository : IRepository<Position>
    {
        IList<PositionDiagram> GetDiagramNodes(string Culture, int value);
        IQueryable<PositionViewModel> GetPositions(string Culture, int CompanyId);
        PositionViewModel ReadPosition(int id, string culure);
        IQueryable<SubperiodViewModel> GetPeriods(int BudgetId ,int PositionId);
        void Add(PosBudget pos);
        void Attach(PosBudget positionBudget);
        DbEntityEntry<PosBudget> Entry(PosBudget positionBudget);
        void Remove(PosBudget PosBd);
        IList<PositionDiagram> GetDiagram(string Culture, int CompanyId);
        void Add(DiagramNode di);
        void Attach(DiagramNode Node);
        DbEntityEntry<DiagramNode> Entry(DiagramNode Node);
        void Remove(DiagramNode Node);
        void Attach(Diagram Node);
        DbEntityEntry<Diagram> Entry(Diagram Node);
        void Remove(Diagram Node);
        void Add(Diagram di);
        PositionDiagram GetRelief(int id, string culture);
    }
}
