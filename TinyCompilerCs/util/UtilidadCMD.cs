/**
 * UtilidadCMD.cs
 * 
 *  Copyrigth (C) 2012: 
 *  Ochoa, Luis lochoa (at unet.edu.ve)
 *  Sanchez, Manuel mbsanchez (at unet.edu.ve)
 *  
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 * MA 02110-1301 USA
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyCompiler.util
{
    class UtilidadCMD
    {
        public static Dictionary<String, String> analizadorSimpleCMD(String[] args)
        {
            Dictionary<String, String> map = new Dictionary<String, String>();
            for (int i = 0; i < args.Length; i++)
            {
                String key = args[i];
                String value = "";

                if(args[i].StartsWith("-")){
                    value = "";
                    if (i+1 < args.Length && !args[i+1].StartsWith("-") && !args[i+1].EndsWith(".tny"))
                    {
                        value = args[i+1]; i++;
                    }
                }
                else if (!map.ContainsKey("--entrada") && key.EndsWith(".tny"))
                {
                    key = "--entrada";
                    value = args[i];
                }
                else
                {
                    key = "--invalido"+i;
                    value = args[i];
                }
                map.Add(key, value);
            }

            return map;
        }
    }
}
