import postcssGlobalData from "@csstools/postcss-global-data";
import postcssCustomMedia from "postcss-custom-media";

export default {
  plugins: [
    postcssGlobalData({
      files: ["../Odyssey.UI/wwwroot/css/base/variables.css"],
    }),
    postcssCustomMedia(),
  ],
};
