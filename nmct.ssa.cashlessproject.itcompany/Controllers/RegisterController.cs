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

            if (sort != null)
            {
                switch (sort)
                {
                    case "allRegisters":
                        return View(organisationsRegisters);

                    case "availableRegisters":
                        return View(organisationsRegisters.Where(or => or.Organisation.ID == 0));

                    default:
                        return View(organisationsRegisters.Where(or => or.Organisation.ID == Convert.ToInt32(sort)));
                }
            }
            else
            {
                return View(organisationsRegisters);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            OrganisationRegister organistationRegister = new OrganisationRegister();
            ViewBag.Organisations = OrganisationDA.GetOrganisations();

            return View(organistationRegister);
        }

        [HttpPost]
        public ActionResult Create(OrganisationRegister organisationRegister)
        {
            if (organisationRegister.Organisation.ID != 0 &&
                !string.IsNullOrEmpty(organisationRegister.Register.RegisterName) && !string.IsNullOrEmpty(organisationRegister.Register.Device) 
                && organisationRegister.Register.PurchaseDate != 0 && organisationRegister.Register.ExpiresDate != 0)
            {
                int registerID = RegisterDA.CreateRegister(organisationRegister);
                RegisterDA.CreateRegisterOrganisation(organisationRegister, registerID);
            }

            return RedirectToAction("Index");
        }
    }
}