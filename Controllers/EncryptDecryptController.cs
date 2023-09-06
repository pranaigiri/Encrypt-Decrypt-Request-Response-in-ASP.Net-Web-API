using EncryptDecrypt.API.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

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
        public IActionResult EncryptData([FromBody] dynamic data)
        {
            try
            {
                string jsonString;

                if (data is JsonElement jsonElement)
                {
                    // data is a JSON object (JsonElement)
                    jsonString = jsonElement.ToString();
                }
                else
                {
                    // data is not a JSON object, so treat it as-is
                    jsonString = JsonConvert.SerializeObject(data);
                }

                string encrypted = _encryptDecryptHelper.Encrypt(jsonString);
                return Ok(encrypted);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
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
