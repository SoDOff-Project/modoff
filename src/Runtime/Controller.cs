namespace modoff.Runtime {
    public class Controller {
        public IActionResult Ok(object obj) {
            return new XmlResult(obj);
        }
    }
}
