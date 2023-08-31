using EncryptDecrypt.API.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace EncryptDecrypt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptDecryptController : ControllerBase
    {
        EncryptDecryptHelper _encryptDecryptHelper;
        public EncryptDecryptController(EncryptDecryptHelper encryptDecryptHelper) 
        { 
            _encryptDecryptHelper = encryptDecryptHelper;
        }


        [HttpPost]
        [Route("Encrypt")]
        public IActionResult EncryptData([FromBody] object data)
        {
            string text = data.ToString()!;
            string encrypted = _encryptDecryptHelper.Encrypt(text);
            return Ok(encrypted);
        }


        [HttpPost]
        [Route("Decrypt")]
        public IActionResult DecryptData([FromBody] object encryptedData)
        {
            string text = encryptedData.ToString()!;
            object decryptedObject = _encryptDecryptHelper.Decrypt(text);

            // Convert dynamic JObject back to formatted JSON string
            if (decryptedObject is JObject jObject)
            {
                return Ok(jObject.ToString());
            }

            return Ok(decryptedObject);
        }
    }
}
