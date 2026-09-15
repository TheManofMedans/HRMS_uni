import{n as e}from"./persianUtils-Cuiil3FL.js";var t=/^\d{10}$/,n=/^\d{10,11}$/,r=/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/,i=`رمز عبور باید:
• حداقل ۸ کاراکتر باشد
• حداقل یک عدد داشته باشد (0-9)
• حداقل یک حرف بزرگ انگلیسی داشته باشد (A-Z)
• حداقل یک حرف کوچک انگلیسی داشته باشد (a-z)
(فقط حروف انگلیسی و اعداد — بدون فاصله)`,a=t=>e(t||``).replace(/\D/g,``),o=e=>r.test(e||``),s=e=>t.test(a(e)),c=e=>!e||n.test(a(e)),l=(e=``)=>({minLength:e.length>=8,hasDigit:/\d/.test(e),hasUpper:/[A-Z]/.test(e),hasLower:/[a-z]/.test(e)});export{a,c as i,s as n,i as o,o as r,l as t};