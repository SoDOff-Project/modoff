using modoff.Runtime;
using modoff.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace modoff.Runtime {
    public class Dispatcher {
        private readonly Dictionary<string, (MethodInfo method, Controller controller)> routes = new Dictionary<string, (MethodInfo method, Controller controller)>();

        public Dispatcher(IEnumerable<Controller> controllers) {
            foreach (var controller in controllers) {
                var methods = controller.GetType()
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance);

                foreach (var method in methods) {
                    var routeAttributes = method.GetCustomAttributes<Route>();
                    foreach (var routeAttr in routeAttributes) {
                        routes[routeAttr.Url] = (method, controller);
                    }
                }
            }
        }

        public string Dispatch(string url, Dictionary<string, string> request) {
            string route = new Uri(url).AbsolutePath.TrimStart('/');
            if (!routes.TryGetValue(route, out var routeInfo)) {
                ModoffLogger.Log($"Route \"{route}\" not found");
                throw new Exception("Route not found");
            }

            ModoffLogger.Log($"ModOff Route Dispatcher: Calling route {route}");

            var (method, controller) = routeInfo;
            var parameters = method.GetParameters();
            var args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++) {
                var param = parameters[i];
                if (request.TryGetValue(param.Name, out string value)) {
                    foreach (var preAction in method.GetCustomAttributes<PreActionAttribute>()) {
                        if (param.Name == preAction.Field)
                            value = preAction.Execute(value);
                    }

                    if (param.ParameterType == typeof(Guid))
                        args[i] = Guid.Parse(value.ToString());
                    else
                    args[i] = Convert.ChangeType(value, param.ParameterType);

                } else {
                    ModoffLogger.Log($"Missing parameter: {param.Name}");
                    throw new Exception($"Missing parameter: {param.Name}");
                }
            }

            IActionResult result = (IActionResult)method.Invoke(controller, args);
            PostActionAttribute attrib = method.GetCustomAttribute<PostActionAttribute>();
            string strResult = result.GetStringData();
            if (attrib != null)
                strResult = attrib.Execute(strResult);
            return strResult;
        }
    }
}
