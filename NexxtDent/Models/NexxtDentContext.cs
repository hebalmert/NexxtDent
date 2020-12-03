using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace NexxtDent.Models
{
    public class NexxtDentContext : DbContext
    {
        public NexxtDentContext() : base("DefaultConnection")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<State> States { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.Register> Registers { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.City> Cities { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.Zone> Zones { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.Identification> Identifications { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.HeadText> HeadTexts { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.SupportText> SupportTexts { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.Tax> Taxes { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.Client> Clients { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.LevelPrice> LevelPrices { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.WorkStation> WorkStations { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.Technical> Technicals { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.WorkCategory> WorkCategories { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.ServiceCategory> ServiceCategories { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.Work> Works { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.Service> Services { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.ToothColor> ToothColors { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.GumColor> GumColors { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.Reception> Receptions { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.ReceptionWork> ReceptionWorks { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.ReceptionLog> ReceptionLogs { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.ReceptionAssign> ReceptionAssigns { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.ReceptionAssignDetail> ReceptionAssignDetails { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.TechnicalWork> TechnicalWorks { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.StateTechnical> StateTechnicals { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.TechnicalWorkDetail> TechnicalWorkDetails { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.Delivery> Deliveries { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.DeliveryDetail> DeliveryDetails { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.Sell> Sells { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.SellDetail> SellDetails { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.CxCBill> CxCBills { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.CxCBillDetail> CxCBillDetails { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.Income> Incomes { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.Outcome> Outcomes { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.OutcomeDetail> OutcomeDetails { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.TechnicalPay> TechnicalPays { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.TechnicalPayDetail> TechnicalPayDetails { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.CxCCanceled> CxCCanceleds { get; set; }

        public System.Data.Entity.DbSet<NexxtDent.Models.TechnicalPorcentage> TechnicalPorcentages { get; set; }
    }
}