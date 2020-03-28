using Merchandising.DTO;
using Merchandising.DTO.Models;
using Merchandising.Enums;
using Merchandising.Helper;
using Merchandising.VM.Portal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Merchandising.Api.Controllers.Portal
{
    /// <summary>
    /// UsersController
    /// </summary>
    public class UsersController : ApiController
    {
        private DbContextModel db = new DbContextModel();

        /// <summary>
        /// GetUserList
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/users/getuserlist")]
        public IHttpActionResult GetUserList(string search = null)
        {
            var user = new List<Users>();
            List<UserListVM> list = new List<UserListVM>();
            //get all users with filter 

            user = db.Users.OrderByDescending(x => x.UserId) .ToList();
            //Search fields
            if (!string.IsNullOrEmpty(search))
            {
                user = user.Where(x =>
                        x.UserName.ToLower().Contains(search.ToLower()) ||
                        x.Role.ToString().ToLower().Contains(search.ToLower()) ||
                        x.Email.ToLower().Contains(search.ToLower()) ||
                        x.LastAccess.ToString().ToLower().Contains(search.ToLower()))
                    .OrderByDescending(x => x.UserId)
                    .ToList();
            }
            if (user.Count > 0)
            {
                list = user.Select(x => new UserListVM()
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    //RoleName = GlobalFunctions.GetAccessRoleValue(Convert.ToInt32(x.Role)),
                    RoleName = db.Roles.FirstOrDefault(b=> b.RoleId == x.Role)?.RoleName,
                    BranchName = db.Branches.FirstOrDefault(b => b.Code == x.BranchCode)?.Name,
                    Status = x.Status,
                    Email = x.Email,
                    ContactNo = x.ContactNo,
                    LastAccess = x.LastAccess
                }).ToList();
            }
            return Ok(list);
        }

        /// <summary>
        /// GetUserInfo
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/users/getuserinfo")]
        public IHttpActionResult GetUserInfo()
        {
            var branch =  db.Branches.Select(y => new { y.Code, y.Name }).Distinct().ToList();
            IEnumerable<SelectListItem> selectList =
                from s in branch
                select new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Code
                };
            var roleData = from AccessRoles e in Enum.GetValues(typeof(AccessRoles))
                           select new SelectListItem
                           {
                               Value = Convert.ToString((int)e),
                               Text = e.ToString()
                           };
            UserVM userAddVm = new UserVM()
            {
                BranchOption = selectList,
                AccessRoleOption = roleData,
                Users = new Users()
            };
            return Ok(userAddVm);
        }
        /// <summary>
        /// GetUsers
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet, System.Web.Http.Route("portal/api/users/{id}")]
        public IHttpActionResult GetUsers(string id)
        {
            Users users =  db.Users.Find(id);
            if (users == null)
            {
                return NotFound();
            }
            users.Password = base64Decode(users.Password);
            return Ok(users);
        } 

        /// <summary>
        /// EditUsers
        /// </summary>
        /// <param name="id"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/users/{id}")]
        public IHttpActionResult EditUsers(string id, [FromBody]Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != users.UserId)
            {
                return BadRequest();
            }
            users.Password = base64Encode(users.Password);
            db.Entry(users).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(users);
        }

        /// <summary>
        /// AuthenticateUser
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPut, System.Web.Http.Route("portal/api/users/authenticateuser/{id}")]
        public IHttpActionResult AuthenticateUser(string id,[FromBody]Users request)
        {
            try
            {
                Users users =  db.Users.FirstOrDefault(x => (x.UserId == id || x.UserName == id) && x.Status == true);
                if (users == null)
                {
                    return BadRequest("Invalid User or User is inactive!.");
                }
                string encrypass = base64Decode(users.Password);
                if (encrypass == request.Password)
                {
                    //Update Last Access every attempt to login
                    users.LastAccess = DateTime.Now;
                    db.Entry(users).State = EntityState.Modified;
                    db.SaveChanges();
                    
                    return Ok(users);
                }
                else
                {
                    return BadRequest("Invalid Password!.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// AddUsers
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost, System.Web.Http.Route("portal/api/users")]
        public IHttpActionResult AddUsers([FromBody]Users users)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //check user if exists
                var check = db.Users.Where(x => x.UserId.ToUpper().Trim() == users.UserId.ToUpper().Trim()).Any();
                if (check)
                {
                    return BadRequest("User already exists! Please create different user.");
                }
                users.Password = base64Encode(users.Password);
                db.Users.Add(users);
                db.SaveChanges();
                return Ok(users);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// DeleteUsers
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpDelete, System.Web.Http.Route("portal/api/users/{id}")]
        public IHttpActionResult DeleteUsers(string id)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return NotFound();
            }

            db.Users.Remove(users);
            db.SaveChanges();

            return Ok(users);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsersExists(string id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
        #region " Valid Methods "
        private bool IsValid(string userid, string password)
        {
            string encrypass = base64Encode(password);
            bool IsValid = false;

            var user = db.Users.FirstOrDefault(u => u.UserId == userid && u.Status == true);
            if (user != null)
            {
                if (user.Password == encrypass)
                {
                    IsValid = true;
                }
            }
            return IsValid;
        }
        public string base64Encode(string sData)
        {
            try
            {
                byte[] encData_byte = new byte[sData.Length];

                encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);

                string encodedData = Convert.ToBase64String(encData_byte);

                return encodedData;

            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        //DECODE 
        public string base64Decode(string sData)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();

                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(sData);

                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);

                char[] decoded_char = new char[charCount];

                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);

                string result = new String(decoded_char);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Decode" + ex.Message);
            }
        }

        #endregion
    }
}