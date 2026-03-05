import postcssGlobalData from "@csstools/postcss-global-data";
import postcssCustomMedia from "postcss-custom-media";
import postcssMixins from "postcss-mixins";

export default {
  plugins: [
    postcssGlobalData({
      files: ["../Haondt.Web.UI/wwwroot/css/variables.css"],
    }),
    postcssMixins({
      mixinsFiles: ["../Haondt.Web.UI/wwwroot/css/variables.css"],
    }),
    postcssCustomMedia(),
  ],
};
