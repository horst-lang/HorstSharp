using System;
using System.Collections.Generic;

namespace Horst
{
    public class Environment
    {
        private readonly Dictionary<string, dynamic> _vars;
        private readonly Dictionary<string, dynamic> _privateVars;
        private readonly Environment _parent;

        public Environment(Environment parent)
        {
            this._parent = parent;
            _vars = parent == null ? new Dictionary<string, dynamic>() : parent._vars;
            _privateVars = new Dictionary<string, dynamic>();
        }

        public Environment Extend()
        {
            return new Environment(this);
        }

        private Environment Lookup(string name)
        {
            Environment scope = this;
            while (scope != null)
            {
                if (scope._privateVars.ContainsValue(name))
                {
                    return scope;
                }

                scope = scope._parent;
            }

            return null;
        }

        public dynamic Get(string name)
        {
            if (_vars.ContainsKey(name))
            {
                return _vars[name];
            }

            throw new Exception($"Undefined variable '{name}'");
        }

        public dynamic Set(string name, dynamic value)
        {
            Environment scope = Lookup(name);
            
            // Maybe remove && parent != null
            if (scope == null)
            {
                Define(name, value);
                //throw new Exception("Undefined variable " + name);
            }

            Environment env = scope ?? this;
            env._privateVars[name] = value;
            return env._vars[name] = value;
        }

        public void Define(string name, dynamic value)
        {
            this._privateVars[name] = value;
            this._vars[name] = value;
        }
    }
}