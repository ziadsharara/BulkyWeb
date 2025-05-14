﻿using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BulkyWeb.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }
        public IActionResult Upsert(int? id)
        {
            if(id == null || id==0)
            {
                return View(new Company());
            }
            else
            {
                Company companyObj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {

                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }
                _unitOfWork.Save();
                TempData["Success"] = "Company created successfully";
                return RedirectToAction("Index", "Company");
            }
            else
            {
                return View(CompanyObj);
            }
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new {data = objCompanyList});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            if(CompanyToBeDeleted==null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}






//public IActionResult Edit(int? id)
//{
//    if(id == null || id ==0)
//    {
//        return NotFound();
//    }
//    Company? CompanyFromDb = _unitOfWork.Company.GetFirstOrDefault(u=>u.Id==id);
//    if(CompanyFromDb == null)
//    {
//        return NotFound();
//    }
//    return View(CompanyFromDb);
//}
//[HttpPost]
//public IActionResult Edit(Company obj)
//{          
//    if (ModelState.IsValid)
//    {
//        _unitOfWork.Company.Update(obj);
//        _unitOfWork.Save();
//        TempData["Success"] = "Company updated successfully";
//        return RedirectToAction("Index", "Company");
//    }
//    return View();
//}
//[HttpPost, ActionName("Delete")]
//public IActionResult DeletePOST(int? id)
//{
//    Company? obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
//    if(obj == null)
//    {
//        return NotFound();
//    }
//    _unitOfWork.Company.Remove(obj);
//    _unitOfWork.Save();
//    TempData["Success"] = "Company deleted successfully";
//    return RedirectToAction("Index", "Company");
//}