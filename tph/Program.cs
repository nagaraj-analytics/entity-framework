using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tph
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new SampleContext();
            var mobileContracts = context.MobileContracts.FirstOrDefault();

        }



        public class SampleContext : DbContext
        {
            public SampleContext () : base("Demo.TPH")
            {

            }
            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {

                base.OnModelCreating(modelBuilder);
            }
            public DbSet<Contract> Contracts { get; set; }
            public DbSet<MobileContract> MobileContracts { get; set; }
            public DbSet<TvContract> TvContracts { get; set; }
            public DbSet<BroadBandContract> BroadBandContracts { get; set; }
        }
        public class Contract
        {
            public int ContractId { get; set; }
            public DateTime StartDate { get; set; }
            public int Months { get; set; }
            public decimal Charge { get; set; }
        }

        public class MobileContract : Contract
        {
            public string MobileNumber { get; set; }
        }

        public class TvContract : Contract
        {
            public PackageType PackageType { get; set; }
        }

        public class BroadBandContract : Contract
        {
            public int DownloadSpeed { get; set; }
        }

        public enum PackageType
        {
            S, M, L, XL
        }

    }
}
