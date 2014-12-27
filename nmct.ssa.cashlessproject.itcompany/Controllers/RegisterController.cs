using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using nmct.ssa.cashlessproject.itcompany.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ssa.cashlessproject.itcompany.Controllers
{
    public class RegisterController : Controller
    {
        public ActionResult Index(string sort)
        {
            List<OrganisationRegister> organisationsRegisters = new List<OrganisationRegister>();
            organisationsRegisters = RegisterDA.GetOrganisationsWithRegisters();

            ViewBag.Organisations = OrganisationDA.GetOrganisations();

            //bij het opstarten en bij het klikken op alle kassa's --> alles weergeven
            if (sort == null || sort  == "allRegisters")
            {
                return View(organisationsRegisters);

            //bij het klikken op beschikbare kassa's --> enkel records weergeven zonder organisatie
            } else if (sort == "availableRegisters") {

                return View(organisationsRegisters.Where(or => or.Organisation.ID == 0));
            
            //standaard sorteren op de aangeklikte organisatie
            } else
            {
                return View(organisationsRegisters.Where(or => or.Organisation.ID == Convert.ToInt32(sort)));
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            OrganisationRegister organistationRegister = new OrganisationRegister();

            //bij het toevoegen van een nieuw register
            //bij gekocht op: dag van vandaag invullen en bij vervalt op: vijf jaar later
            RegisterCompany register = new RegisterCompany() { PurchaseDate = DateTime.Today, ExpiresDate = DateTime.Today.AddYears(5) };
            organistationRegister.Register = register;

            ViewBag.Organisations = OrganisationDA.GetOrganisations();

            return View(organistationRegister);
        }

        [HttpPost]
        public ActionResult Create(OrganisationRegister organisationRegister)
        {
            //hier kan ik niet werken met ModelState.IsValid doordat ik in mijn query niet alle waarden ophaal van het model OrganisationRegister
            if (!string.IsNullOrEmpty(organisationRegister.Register.RegisterName) 
                && !string.IsNullOrEmpty(organisationRegister.Register.Device) 
                && organisationRegister.Register.PurchaseDate != null
                && organisationRegister.Register.ExpiresDate != null)
            {
                //als er een organisatie is toegekend, een record plaatsen in in de tussentabel
                if (organisationRegister.Organisation.ID == 0)
                {
                    organisationRegister.FromDate = new DateTime(1970, 1, 1, 12, 0, 0);
                    organisationRegister.UntilDate = new DateTime(1970, 1, 1, 12, 0, 0);
                }
                else
                {
                    //dag van vandaag en niet purchase date, kassa kan bv. gisteren aangekocht zijn en vandaag pas toegekend zijn
                    organisationRegister.FromDate = DateTime.Today;
                    organisationRegister.UntilDate = organisationRegister.Register.ExpiresDate;
                }

                int registerID = RegisterDA.CreateRegister(organisationRegister.Register);
                RegisterDA.CreateRegisterOrganisation(organisationRegister, registerID);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");

            OrganisationRegister organisationRegister = RegisterDA.GetRegisterByID(id.Value);

            //hier controleren op ID en niet op null, door gebruik te maken van RIGHT JOIN is organisationRegister nooit null
            if (organisationRegister.ID == 0)
                return RedirectToAction("Index");

            ViewBag.Organisations = OrganisationDA.GetOrganisations();

            return View(organisationRegister);
        }

        public ActionResult Edit(OrganisationRegister organisationRegister)
        {
            //nog extra controle: geldig id

            //bestaand record ophalen
            OrganisationRegister orOud = RegisterDA.GetRegisterByID(organisationRegister.ID);

            ////van niet beschikbaar naar beschikbaar (geen organisatie)
            if (orOud.Organisation.ID == 0 && organisationRegister.Organisation.ID != 0)
            {
                organisationRegister.FromDate = DateTime.Today;
                organisationRegister.UntilDate = orOud.Register.ExpiresDate;
            }

            //van beschikbaar naar niet beschikbaar (wel een organisatie)
            else if (orOud.Organisation.ID != 0 && organisationRegister.Organisation.ID == 0)
            {
                organisationRegister.FromDate = new DateTime(1970, 1, 1, 12, 0, 0);
                organisationRegister.UntilDate = new DateTime(1970, 1, 1, 12, 0, 0);
            }
            else
            {
                organisationRegister.FromDate = orOud.FromDate;
                organisationRegister.UntilDate = orOud.UntilDate;
            }

            RegisterDA.UpdateRegisterOrganisation(organisationRegister);

            return RedirectToAction("Index");
        }
    }
}