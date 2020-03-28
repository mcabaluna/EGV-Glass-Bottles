using Merchandising.DTO.Models;
using Merchandising.Enums;
using Merchandising.Portal.Models;
using Merchandising.VM.Portal;
using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Merchandising.Portal.Controllers
{
    public class SequenceTableController : Controller
    {
        #region " View Sequence Table "
        public ActionResult Index()
        {
            var obj = MerchandisingApiWrapper.Get<List<SequenceDocument>>(
                                       typeof(SequenceDocument).Name + "/getsequencedocument");
            var objbranch = MerchandisingApiWrapper.Get<List<Branch>>(
                                   typeof(Branch).Name + "/getbranchinfo");

            var seqdocument = obj.Select(y => new { y.ObjectCode, y.DocumentName, y.DocType, y.DocSubType }).Distinct().ToList();
            IEnumerable<SelectListItem> seqdoclist =
                from s in seqdocument
                select new SelectListItem
                {
                    Text = s.DocumentName,
                    Value = s.ObjectCode + "|" + s.DocType + "|" + s.DocSubType
                };
            var branch = objbranch.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> branchlist =
                from s in branch
                select new SelectListItem
                {
                    Text = s.Code + " - " + s.Name,
                    Value = s.Code
                };
            var doctypelist = new List<SelectListItem>()
            {
                new SelectListItem{ Text = "TRANSACTIONAL",  Value = "TRANSACTIONAL"},
                new SelectListItem{   Text = "MASTERDATA",  Value = "MASTERDATA"  }
            };
            var statusData = from StatusType e in Enum.GetValues(typeof(StatusType))
                             select new SelectListItem
                             {
                                 Value = Convert.ToString((int)e),
                                 Text = e.ToString()
                             };

            SequenceVM sequenceVM = new SequenceVM()
            {
                DocumentOption = new SelectList(seqdoclist, "Value", "Text"),
                DocumentTypeOption = new SelectList(doctypelist, "Value", "Text"),
                BranchOption = new SelectList(branchlist, "Value", "Text"),
                StatusOption = new SelectList(statusData, "Value", "Text"),
                SeqDocument = new SequenceDocument(),
                Sequence = new SequenceTable()
            };
            return View(sequenceVM);
        }
        public ActionResult GetSequenceTable(int id)
        {
            var obj = MerchandisingApiWrapper.Get<SequenceTable>(
                                  typeof(SequenceTable).Name + $"/{id}");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSequenceInfo()
        {
            var objitem = MerchandisingApiWrapper.Get<List<SequenceTable>>(
                         typeof(SequenceTable).Name + "/getsequenceinfo");
            return Json(objitem, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(GridParams g, string search = null)
        {
            var obj = MerchandisingApiWrapper.Get<List<SequenceListVM>>(
                typeof(SequenceTable).Name + "/getsequencelist" + $"?search={search}");

            //return Json(new {data = obj}, JsonRequestBehavior.AllowGet);
            return Json(new GridModelBuilder<SequenceListVM>(obj.AsQueryable(), g)
            {
                KeyProp = o => o.Id,// needed for Entity Framework | nesting | tree | api
                Map = o => new
                {
                    o.Id,
                    o.ObjectCode,
                    o.Document,
                    o.DocumentType,
                    o.DocSubType,
                    o.Series,
                    o.DefaultSeries,
                    o.InitialNum,
                    o.NextNumber,
                    o.LastNum
                }
            }.Build());
        }
        #endregion
        #region "View Sequence Document"
        public ActionResult GetSequenceDocument()
        {
            Api.SequenceDocumentController sequence = new Api.SequenceDocumentController();
            var obj = sequence.GetSequentDocumentInfo();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDocuments()
        {
            Api.SequenceDocumentController sequence = new Api.SequenceDocumentController();
            var obj = sequence.GetSequentDocumentInfo();

            var objitem = MerchandisingApiWrapper.Get<List<SequenceTable>>(
             typeof(SequenceTable).Name + "/getsequenceinfo");

            var result = obj.Where(x => !objitem.Exists(b => b.ObjectCode == x.ObjectCode)).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region " Sequence Table Posting "
        [HttpPost]
        public ActionResult Save(SequenceTable entity)
        {
            Api.SequenceTableController sequence = new Api.SequenceTableController();
            var obj = sequence.AddSequenceTable(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult Update(SequenceTable entity)
        {
            Api.SequenceTableController sequence = new Api.SequenceTableController();
            var obj = sequence.EditSequenceTable(entity.Id, entity);
            return new JsonResult { Data = obj };
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Api.SequenceTableController sequence = new Api.SequenceTableController();
            var obj = sequence.DeleteSequenceTable(id);
            return new JsonResult { Data = obj };
        }
        #endregion
        #region " Sequence Document Posting "
        [HttpPost]
        public ActionResult SaveDocument(SequenceDocument entity)
        {
            Api.SequenceDocumentController sequence = new Api.SequenceDocumentController();
            var obj = sequence.AddSequenceDocument(entity);
            return new JsonResult { Data = obj };
        }
        [HttpPut]
        public ActionResult UpdateDocument(SequenceDocument entity)
        {
            //Api.SequenceDocumentController sequence = new Api.SequenceDocumentController();
            //var obj = sequence.EditSequenceDocument(entity.Document, entity);
            //return new JsonResult { Data = obj };
            return new JsonResult { Data = "" };
        }
        [HttpDelete]
        public ActionResult DeleteDocument(string id)
        {
            Api.SequenceDocumentController sequence = new Api.SequenceDocumentController();
            var obj = sequence.DeleteSequenceDocument(id);
            return new JsonResult { Data = obj };
        }
        #endregion
        #region "Check Sequence"
        public ActionResult CheckSequence(int objectcode, int objectcode2 = 0)
        {
            var success = false;
            var obj = MerchandisingApiWrapper.Get<List<SequenceTable>>(
                          typeof(SequenceTable).Name + "/getsequenceinfo");
            var check = obj.Any(x => (x.ObjectCode == objectcode || x.ObjectCode == objectcode2) && x.Lines.Count > 0 && x.Lines.Any(b => b.Locked == false));
            if (check)
                success = true;
            return Json(success, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCurrentSequence(int series, int objectcode)
        {
            SequenceTable result = new SequenceTable();
            //string seriesval = string.Empty;
            List<string> seriesval = new List<string>();
            var obj = MerchandisingApiWrapper.Get<List<SequenceTable>>(
                          typeof(SequenceTable).Name + "/getsequenceinfo");

            result = obj.SingleOrDefault(x => x.ObjectCode == objectcode);
            if (result != null)
            {
                var newres = result.Lines.SingleOrDefault(x => x.ObjectCode == objectcode && x.Series == series);
                if (newres != null)
                {
                    if (newres.NumSize > 0)
                    {
                        var nextnumber = newres.NextNumber.ToString();
                        var numsize = newres.NumSize;
                        var concatnum = nextnumber.PadLeft(nextnumber.Length + numsize - nextnumber.Length, '0');
                        //Master Data
                        //seriesval = newres.LastStr != null ? newres.BeginStr + concatnum + "_" + newres.LastStr :
                        //                  newres.BeginStr + concatnum;
                        seriesval.Add(newres.LastStr != null ? newres.BeginStr + concatnum + "_" + newres.LastStr :
                                          newres.BeginStr + concatnum);
                    }
                    else
                    {
                        //Transactional

                        seriesval.Add(newres.LastStr != null ? newres.BeginStr + newres.NextNumber + "_" + newres.LastStr :
                                      newres.BeginStr + newres.NextNumber);
                        seriesval.Add(newres.NextNumber.ToString());
                        //seriesval = newres.NextNumber.ToString();
                    }
                    seriesval.Add(newres.BranchCode);
                }
            }
            return Json(seriesval, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSeriesBasedOnObject(int objectcode)
        {
            var objsequence = MerchandisingApiWrapper.Get<List<SequenceTable>>(
                      typeof(SequenceTable).Name + "/getsequenceinfo");

            var seq = objsequence.SingleOrDefault(x => x.ObjectCode == objectcode);
            var series = seq.Lines
                            .Select(y =>
                                new
                                {
                                    y.Series,
                                    y.SeriesName,
                                    y.BeginStr,
                                    y.NextNumber,
                                    y.LastStr,
                                    y.ObjectCode,
                                    y.BranchCode,
                                    y.Indicator,
                                    seq.DefaultSeries
                                }).ToList();
            return Json(series, JsonRequestBehavior.AllowGet);
        }
        public IEnumerable<SequenceTableLines> GetSequenceSeries(int objectcode)
        {
            var objsequence = MerchandisingApiWrapper.Get<List<SequenceTable>>(
                      typeof(SequenceTable).Name + "/getsequenceinfo");

            var seq = objsequence.Where(x => x.ObjectCode == objectcode).ToList();
            var series = seq.SelectMany(x => x.Lines).ToList();
            return series;
        }
        public ActionResult GetSeries(string docsubtype)
        {
            var objsequence = MerchandisingApiWrapper.Get<List<SequenceTable>>(
                      typeof(SequenceTable).Name + "/getsequenceinfo");

            var seq = objsequence.SingleOrDefault(x => x.DocSubType == docsubtype);
            var series = seq.Lines
                            .Select(y =>
                                new
                                {
                                    y.Series,
                                    y.SeriesName,
                                    y.BeginStr,
                                    y.NextNumber,
                                    y.LastStr,
                                    y.ObjectCode,
                                    seq.DefaultSeries
                                }).ToList();
            return Json(series, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}