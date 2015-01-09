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
    [Authorize(Roles = "Administrator")]
    public class RegisterController : Controller
    {
        //geen httpget of httppost bijzetten --> eerste keer laden: get, inladen met dropdownlist --> post
        public ActionResult Index(string sort)
        {
            List<OrganisationRegister> organisationsRegisters = new List<OrganisationRegister>();
            organisationsRegisters = RegisterDA.GetOrganisationsWithRegisters();

            //vereniging die geen vereniging is niet in viewbag plaatsen
            //--> wordt handmatig in dropdownlist (view) geplaatst voor beter naamgeving
            ViewBag.Organisations = OrganisationDA.GetOrganisations().Where(r => r.ID != -1);

            //bij het opstarten en bij het klikken op alle kassa's --> alles weergeven
            if (sort == null || sort  == "allRegisters")
            {
                ViewBag.titleRegisters = "Alle kassa's";
                return View(organisationsRegisters);

            //bij het klikken op beschikbare kassa's --> enkel records weergeven zonder organisatie
            } else if (sort == "availableRegisters")
            {
                ViewBag.titleRegisters = "Beschikbare kassa's";
                return View(organisationsRegisters.Where(or => or.Organisation.ID == -1));
            
            //standaard sorteren op de aangeklikte organisatie
            } else
            {
                Organisation organisation = OrganisationDA.GetOrganisationByID(Convert.ToInt32(sort));
                if (organisation == null)
                    return RedirectToAction("Index");

                ViewBag.titleRegisters = "Kassa's van vereniging " + organisation.OrganisationName;
                return View(organisationsRegisters.Where(or => or.Organisation.ID == Convert.ToInt32(sort)));
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            OrganisationRegister organistationRegister = new OrganisationRegister();

            //bij het toevoegen van een nieuw register
            //bij gekocht op: dag van vandaag invullen en bij vervalt op: vijf jaar later
            RegisterCompany register = new RegisterCompany() {
                PurchaseDate = DateTime.Today,
                ExpiresDate = DateTime.Today.AddYears(5) 
            };

            organistationRegister.Register = register;

            ViewBag.Organisations = OrganisationDA.GetOrganisations();

            return View(organistationRegister);
        }

        [HttpPost]
        public ActionResult Create(OrganisationRegister organisationRegister)
        {
            //hier kan ik niet werken met ModelState.IsValid doordat ik in niet alle waarden ophaal van het model OrganisationRegister
            if (organisationRegister.Organisation.ID != 0
                && !string.IsNullOrEmpty(organisationRegister.Register.RegisterName) 
                && !string.IsNullOrEmpty(organisationRegister.Register.Device) 
                && organisationRegister.Register.PurchaseDate != null
                && organisationRegister.Register.ExpiresDate != null)
            {
                //als er geen organisatie is toegekend, een record plaatsen in de tussentabel met "default" datums
                if (organisationRegister.Organisation.ID == -1)
                {
                    organisationRegister.FromDate = new DateTime(1970, 1, 1, 12, 0, 0);
                    organisationRegister.UntilDate = new DateTime(1970, 1, 1, 12, 0, 0);
                }
                //indien er wel een organisatie is toegekend, dag van en tot datum instellen
                //dag van vandaag nemen voor "van" datum en niet gekocht op datum: kassa kan bv. gisteren aangekocht zijn en vandaag pas toegekend zijn
                //voor "tot" datum vervaldag nemen: wanneer kassa vervalt, is de kassa ook niet meer in het bedrijf
                else
                {
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

            if (organisationRegister.ID == 0)
                return RedirectToAction("Index");

            ViewBag.Organisations = OrganisationDA.GetOrganisations();

            return View(organisationRegister);
        }

        [HttpPost]
        public ActionResult Edit(OrganisationRegister organisationRegister)
        {
            //nog extra controle: geldig id
            if (organisationRegister.ID == 0)
                return RedirectToAction("Index");

            //bestaand record ophalen
            OrganisationRegister orOud = RegisterDA.GetRegisterByID(organisationRegister.ID);

            //controle of organisatie wel bestaat
            if (orOud.Organisation == null)
                return RedirectToAction("Index");

            //van beschikbaar (geen organisatie) naar niet beschikbaar (wel een organisatie)
            if (orOud.Organisation.ID == -1 && organisationRegister.Organisation.ID != -1)
            {
                organisationRegister.FromDate = DateTime.Today;
                organisationRegister.UntilDate = orOud.Register.ExpiresDate;
            }

            //van niet beschikbaar (wel een organisatie) naar beschikbaar (geen organisatie)
            else if (orOud.Organisation.ID != -1 && organisationRegister.Organisation.ID == -1)
            {
                organisationRegister.FromDate = new DateTime(1970, 1, 1, 12, 0, 0);
                organisationRegister.UntilDate = new DateTime(1970, 1, 1, 12, 0, 0);
            }

            //bv. veranderen van organisatie: van en tot datum behouden
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