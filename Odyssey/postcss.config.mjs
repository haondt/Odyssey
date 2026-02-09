import postcssGlobalData from "@csstools/postcss-global-data";
import postcssCustomMedia from "postcss-custom-media";

export default {
  plugins: [
    postcssGlobalData({
      files: ["../Haondt.Web.UI/wwwroot/css/variables.css"],
    }),
    postcssCustomMedia(),
  ],
};
