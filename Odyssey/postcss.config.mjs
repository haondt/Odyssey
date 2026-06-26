import postcssGlobalData from "@csstools/postcss-global-data";
import postcssCustomMedia from "postcss-custom-media";
import postcssMixins from "postcss-mixins";
import postcssNesting from "postcss-nesting";
import postcssIsPseudoClass from '@csstools/postcss-is-pseudo-class';
import postCSSReplace from "postcss-replace";

export default {
  plugins: [
    postcssGlobalData({
      files: ["../Haondt.Web.UI/wwwroot/css/variables.css"],
    }),
    postcssMixins({
      mixinsFiles: ["../Haondt.Web.UI/wwwroot/css/variables.css"],
    }),
    postcssCustomMedia(),
    postcssNesting(),
    postCSSReplace({
        pattern: /(::-csstools-invalid-)/,
        data: { '::-csstools-invalid-': '::'}
    }),
    postcssIsPseudoClass(),
  ],
};
