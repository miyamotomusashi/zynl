﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using web.Areas.Admin.Filters;
using web.Areas.Admin.Helpers;
using BLL.LanguageBL;
using BLL.ProductBL;
//using BLL.ProductBL;
using DAL.Entities;

namespace web.Areas.Admin.Controllers
{
    [AuthenticateUser]
    public class ProductGroupController : Controller
    {
        public ActionResult Index()
        {
            string lang = FillLanguagesList();
            web.Areas.Admin.Models.VMProductGroupModel grouplist = new Models.VMProductGroupModel();
            grouplist.ProductGroup = ProductManager.GetProductGroupList(lang);
            return View(grouplist);
        }


        [HttpPost]
        public ActionResult Index(string txtname,int topProductGroupId)
        {
            string lang = FillLanguagesList();
            if (ModelState.IsValid)
            {
                ProductGroup model = new ProductGroup();
                model.GroupName = txtname;
                model.Language = "tr";
                model.PageSlug = Utility.SetPagePlug(txtname);
                model.TopProductId = topProductGroupId;
                ViewBag.ProcessMessage = ProductManager.AddProductGroup(model);
                web.Areas.Admin.Models.VMProductGroupModel grouplist = new Models.VMProductGroupModel();
                grouplist.ProductGroup = ProductManager.GetProductGroupList(lang);
                return View(grouplist);
            }
            return View();
        }


        public ActionResult EdtiGroup() 
        {
            var lang = FillLanguagesList();

               

                if(RouteData.Values["id"]!=null)
                {
                    int nid=0;
                    bool isnumber=int.TryParse(RouteData.Values["id"].ToString(),out nid);
                    if (isnumber)
                    {
                        ProductGroup editnews = ProductManager.GetGroupById(nid);
                        return View(editnews);
                    }
                    else
                        return View();
                }
                else
                    return View();
         }
    
        [HttpPost]
        public ActionResult EdtiGroup(ProductGroup model, HttpPostedFileBase uploadfile)
        {
            var languages = LanguageManager.GetLanguages();
            var list = new SelectList(languages, "Culture", "Language");
            ViewBag.LanguageList = list;
            if (ModelState.IsValid)
            {
                //ProductGroup model = new ProductGroup();
               // model.GroupName = txtname;
                //model.Language = drplanguage;
                if (uploadfile != null && uploadfile.ContentLength > 0)
                {
                    Random random = new Random();
                    int rand = random.Next(1000, 99999999);
                    new ImageHelper(280, 240).SaveThumbnail(uploadfile, "/Content/images/productcategory/", Utility.SetPagePlug(model.GroupName) + "_" + rand + Path.GetExtension(uploadfile.FileName));
                    model.GroupImage = "/Content/images/productcategory/" + Utility.SetPagePlug(model.GroupName) + "_" + rand + Path.GetExtension(uploadfile.FileName);
                }
                if (RouteData.Values["id"] != null)
                {
                    int nid = 0;
                    bool isnumber = int.TryParse(RouteData.Values["id"].ToString(), out nid);
                    if (isnumber)
                    {
                        model.PageSlug = Utility.SetPagePlug(model.GroupName);
                        model.ProductGroupId = nid;
                        ViewBag.ProcessMessage = ProductManager.EditProductGroup(model);
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ProcessMessage = false;
                        return View(model);
                    }
                }

            }
            return View();
        }




        public void UpdateRecord(int id, string name)
        {
            string clearname = name.Replace("%47", "\'");
            string pageslug = Utility.SetPagePlug(clearname);
            ProductManager.EditProductGroup(id, clearname, pageslug);
        }


        public JsonResult GroupEditStatus(int id)
        {
            return Json(ProductManager.UpdateGroupStatus(id));
        }

        public JsonResult DeleteRecord(int id)
        {
            return Json(ProductManager.DeleteGroup(id));
        }


        public JsonResult SortRecords(string list)
        {
            JsonList psl = (new JavaScriptSerializer()).Deserialize<JsonList>(list);
            string[] idsList = psl.list;
            bool issorted = ProductManager.SortRecords(idsList);
            return Json(issorted);


        }

        public class JsonList
        {
            public string[] list { get; set; }
        }


        string FillLanguagesList()
        {
            string lang = "";
            if (RouteData.Values["lang"] == null)
                lang = "tr";
            else lang = RouteData.Values["lang"].ToString();

            var languages = LanguageManager.GetLanguages();
            var list = new SelectList(languages, "Culture", "Language", lang);
            ViewBag.LanguageList = list;
            return lang;
        }
    }
}
