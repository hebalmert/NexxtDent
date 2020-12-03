using NexxtDent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NexxtDent.Classes
{
    public class ComboHelper : IDisposable
    {
        private static NexxtDentContext db = new NexxtDentContext();

        //Combos de Compañias
        public static List<Company> GetCompanies()
        {
            var companies = db.Companies.ToList();
            companies.Add(new Company
            {
                CompanyId = 0,
                Compania = @Resources.Resource.ComboSelect,
            });
            return companies.OrderBy(d => d.Compania).ToList();
        }

        //Combos de Ciudades
        public static List<City> GetCities(int companyid)
        {
            var cities = db.Cities.Where(c => c.CompanyId == companyid).ToList();
            cities.Add(new City
            {
                CityId = 0,
                Ciudad = @Resources.Resource.ComboSelect,
            });
            return cities.OrderBy(d => d.Ciudad).ToList();
        }

        //Combos de Identification
        public static List<Identification> GetIdentifications(int companyid)
        {
            var identification = db.Identifications.Where(c => c.CompanyId == companyid).ToList();
            identification.Add(new Identification
            {
                IdentificationId = 0,
                TipoDocumento = @Resources.Resource.ComboSelect,
            });
            return identification.OrderBy(d => d.TipoDocumento).ToList();
        }


        //Combos de Niveles de Precio

        public static List<LevelPrice> GetLevelPrice()
        {
            var levelprices = db.LevelPrices.ToList();
            levelprices.Add(new LevelPrice
            {
                LevelPriceId = 0,
                NivelPrecio = @Resources.Resource.ComboSelect,
            });
            return levelprices.OrderBy(d => d.NivelPrecio).ToList();
        }

        //Combos de Ciudades

        public static List<City> GetCity(int companyId)
        {
            var cities = db.Cities.Where(c => c.CompanyId == companyId).ToList();
            cities.Add(new City
            {
                CityId = 0,
                Ciudad = @Resources.Resource.ComboSelect,
            });
            return cities.OrderBy(d => d.Ciudad).ToList();
        }

        //Combos de Zonas
        public static List<Zone> GetZone(int companyid)
        {
            var zones = db.Zones.Where(c => c.CompanyId == companyid).ToList();
            zones.Add(new Zone
            {
                ZoneId = 0,
                Zona = @Resources.Resource.ComboSelect,
            });
            return zones.OrderBy(d => d.Zona).ToList();
        }

        //Combos de Estaciones de Trabajo

        public static List<WorkStation> GetWorkstation(int companyId)
        {
            var workstations = db.WorkStations.Where(c => c.CompanyId == companyId).ToList();
            workstations.Add(new WorkStation
            {
                WorkStationId = 0,
                Estacion = @Resources.Resource.ComboSelect,
            });
            return workstations.OrderBy(d => d.Estacion).ToList();
        }

        //Combos de  Categoria Trabajos
        public static List<WorkCategory> GetCategoryWork(int companyid)
        {
            var workcategories = db.WorkCategories.Where(c => c.CompanyId == companyid).ToList();
            workcategories.Add(new WorkCategory
            {
                WorkCategoryId = 0,
                Categoria = @Resources.Resource.ComboSelect,
            });
            return workcategories.OrderBy(d => d.Categoria).ToList();
        }

        //Combos de  Impuestos
        public static List<Tax> GetTax(int companyid)
        {
            var taxes = db.Taxes.Where(c => c.CompanyId == companyid).ToList();
            taxes.Add(new Tax
            {
                TaxId = 0,
                Impuesto = @Resources.Resource.ComboSelect,
            });
            return taxes.OrderBy(d => d.Impuesto).ToList();
        }

        //Combos de  Categoria Servicios
        public static List<ServiceCategory> GetCategoryService(int companyid)
        {
            var servicecategories = db.ServiceCategories.Where(c => c.CompanyId == companyid).ToList();
            servicecategories.Add(new ServiceCategory
            {
                ServiceCategoryId = 0,
                Categoria = @Resources.Resource.ComboSelect,
            });
            return servicecategories.OrderBy(d => d.Categoria).ToList();
        }

        public static List<Client> GetClient(int companyId)
        {
            var clincis = db.Clients.Where(c => c.CompanyId == companyId).ToList();
            clincis.Add(new Client
            {
                ClientId = 0,
                Cliente = @Resources.Resource.ComboSelect,
            });
            return clincis.OrderBy(d => d.Cliente).ToList();
        }

        public static List<ToothColor> GetToothcolor(int companyId)
        {
            var toothcolor = db.ToothColors.Where(c => c.CompanyId == companyId && c.Activo == true).ToList();
            toothcolor.Add(new ToothColor
            {
                ToothColorId = 0,
                ColorDiente = @Resources.Resource.ComboSelect,
            });
            return toothcolor.OrderBy(d => d.ColorDiente).ToList();
        }

        public static List<GumColor> GetGumcolor(int companyId)
        {
            var gumcolor = db.GumColors.Where(c => c.CompanyId == companyId && c.Activo == true).ToList();
            gumcolor.Add(new GumColor
            {
                GumColorId = 0,
                ColorEncia = @Resources.Resource.ComboSelect,
            });
            return gumcolor.OrderBy(d => d.ColorEncia).ToList();
        }

        //Combos de  Trabajos
        public static List<Work> GetWork(int companyid)
        {
            var works = db.Works.Where(c => c.CompanyId == companyid).ToList();
            works.Add(new Work
            {
                WorkId = 0,
                Trabajo = @Resources.Resource.ComboSelect,
            });
            return works.OrderBy(d => d.Trabajo).ToList();
        }

        //Combos de  Servicios
        public static List<Technical> GetTechnical(int companyid)
        {
            var technicals = db.Technicals.Where(c => c.CompanyId == companyid && c.Activo == true).ToList();
            technicals.Add(new Technical
            {
                TechnicalId = 0,
                FullName = @Resources.Resource.ComboSelect,
            });
            return technicals.OrderBy(d => d.FullName).ToList();
        }


        //Combos de  Trabajos
        public static List<Service> GetService(int companyid)
        {
            var services = db.Services.Where(c => c.CompanyId == companyid).ToList();
            services.Add(new Service
            {
                ServiceId = 0,
                Servicio = @Resources.Resource.ComboSelect,
            });
            return services.OrderBy(d => d.Servicio).ToList();
        }

        //Combos de  Trabajos
        public static List<StateTechnical> GetStateTechnical()
        {
            var statetechnical = db.StateTechnicals.ToList();
            statetechnical.Add(new StateTechnical
            {
                StateTechnicalId = 0,
                EstadoTecnico = @Resources.Resource.ComboSelect,
            });
            return statetechnical.OrderBy(d => d.StateTechnicalId).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}