string photo = "C://captcha.jpg".Trim(); // Путь к каптче в формате JPG
string ip_port = "127.0.0.3:81"; // IP:PORT Капмонстра
string key_capmonstr = @"123456"; // Ключ Капмонстра
string CapMonsterModule = @"module_XRB"; // Модуль на который отправляем каптчу

string url_post = string.Format(@"http://{0}/in.php",ip_port);
string boundary = DateTime.Now.Ticks.ToString("x");
string contentType = "multipart/form-data";
var fileInfo = new System.IO.FileInfo(photo); 

#region Подготовка запроса
// Метод
string[] method = new[] { string.Format("--{0}\r\n", boundary),	string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n", "method"), string.Format("{0}\r\n", "post") };
string out_method = string.Join(string.Empty, method);

// Ключ
string[] key = new[] { string.Format("--{0}\r\n", boundary),string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n", "key"),	string.Format("{0}\r\n", key_capmonstr)};
string out_key = string.Join(string.Empty, key);

// Модуль
string[] CMModule = new[] {	string.Format("--{0}\r\n", boundary), string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n", "CapMonsterModule"),	string.Format("{0}\r\n", CapMonsterModule) };
string out_CMModule = string.Join(string.Empty, CMModule);

// Файл
string[] captcha_file = new[] {
string.Format("--{0}\r\n", boundary),
string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n", "file", fileInfo.Name),
string.Format("Content-Type: {0}\r\n\r\n", "image/jpg"),
string.Format("{0}\r\n", fileInfo.FullName)
};
string out_captcha_file = string.Join(string.Empty, captcha_file);
string data = out_method + out_key + out_CMModule + out_captcha_file;
#endregion
// Отправка каптчи на капмонстр
string post = ZennoPoster.HttpPost( url_post, data, contentType, string.Empty, "UTF-8", ZennoLab.InterfacesLibrary.Enums.Http.ResponceType.BodyOnly, 30000, string.Empty, "ZP", false, 0, new [] {string.Empty });
// Формируем URL на получение результата распознавания
string url_get = string.Format(@"http://{0}/res.php?key={1}&action=get&id={2}", ip_port, key_capmonstr, post.Split('|')[1]).ToString();
//Отправляем запрос на распознавание
string get = ZennoPoster.HttpGet( url_get, string.Empty, "UTF-8", ZennoLab.InterfacesLibrary.Enums.Http.ResponceType.BodyOnly, 30000, string.Empty,"ZP", true, 0, new[]{string.Empty});
return get;
