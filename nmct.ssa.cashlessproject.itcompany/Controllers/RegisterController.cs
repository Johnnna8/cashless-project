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

            if (sort == null || sort  == "allRegisters")
            {
                return View(organisationsRegisters);
            } else {
                return View(organisationsRegisters.Where(or => or.Organisation.ID == Convert.ToInt32(sort)));
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            OrganisationRegister organistationRegister = new OrganisationRegister();

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
                if (organisationRegister.Organisation.ID != 6)
                {
                    //dag van vandaag en niet purchase date, kassa kan bv. gisteren aangekocht zijn en vandaag pas toegekend zijn
                    organisationRegister.FromDate = DateTime.Today;
                    organisationRegister.UntilDate = organisationRegister.Register.ExpiresDate;
                }
                else
                {
                    organisationRegister.FromDate = new DateTime(1970, 1, 1, 12, 0, 0);
                    organisationRegister.UntilDate = new DateTime(1970, 1, 1, 12, 0, 0);
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

            if (organisationRegister == null)
                return RedirectToAction("Index");

            ViewBag.Organisations = OrganisationDA.GetOrganisations();

            return View(organisationRegister);
        }

        public ActionResult Edit(OrganisationRegister organisationRegister)
        {
            //ok: als de vereniging hetzelfde blijft blijven de from en until onaangepast
            //ok: als er geen vereging wordt toegekent staan de from en until op default
            //niet ok: als je van vereniging veranderd blijven de from en until onaangepast --> from moet aangepast zijn op de dag van vandaag, kan wel manueel aangepast worden

            if (organisationRegister.Organisation.ID != 0 
                && organisationRegister.Register.ID != 0
                && !string.IsNullOrEmpty(organisationRegister.Register.RegisterName) 
                && !string.IsNullOrEmpty(organisationRegister.Register.Device))
            {

                if (organisationRegister.Organisation.ID == 6)
                {
                    organisationRegister.FromDate = new DateTime(1970, 1, 1, 12, 0, 0);
                    organisationRegister.UntilDate = new DateTime(1970, 1, 1, 12, 0, 0);
                }

                RegisterDA.EditRegister(organisationRegister.Register);
                RegisterDA.DeleteRegisterOrganisation(organisationRegister.Register.ID);
                RegisterDA.CreateRegisterOrganisation(organisationRegister, organisationRegister.Register.ID);
            }

            return RedirectToAction("Index");
        }
    }
}