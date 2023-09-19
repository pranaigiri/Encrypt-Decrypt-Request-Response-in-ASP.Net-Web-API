using EncryptDecrypt.API.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EncryptDecrypt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptDecryptCryptoJsController : ControllerBase
    {
        EncryptDecryptHelperCryptoJs _encryptDecryptHelperCryptoJs;
        public EncryptDecryptCryptoJsController(EncryptDecryptHelperCryptoJs encryptDecryptHelperCryptoJs)
        {
            _encryptDecryptHelperCryptoJs = encryptDecryptHelperCryptoJs;
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

                string encrypted = _encryptDecryptHelperCryptoJs.EncryptStringAES(jsonString);
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
            object decryptedObject = _encryptDecryptHelperCryptoJs.DecryptStringAES(text);

            // Convert dynamic JObject back to formatted JSON string
            if (decryptedObject is JObject jObject)
            {
                return Ok(jObject.ToString());
            }

            return Ok(decryptedObject);
        }
    }
}
