'use strict'
const serverApi = "http://localhost:9565" //api server address

// All configuration item explanations can be find in https://cli.vuejs.org/config/
module.exports = {
  /**
   * You will need to set publicPath if you plan to deploy your site under a sub path,
   * for example GitHub Pages. If you plan to deploy your site to https://foo.github.io/bar/,
   * then publicPath should be set to "/bar/".
   * In most cases please use '/' !!!
   * Detail: https://cli.vuejs.org/config/#publicpath
   */
  publicPath: '/',
  outputDir: 'dist',
  assetsDir: 'static',
  lintOnSave: process.env.NODE_ENV === 'development',
  productionSourceMap: false,
  devServer: {
    open: true,
    overlay: {
      warnings: false,
      errors: true
    },
    proxy: {
      '/api': {
        target: `${serverApi}`,
        changeOrigin: true,
        pathRewrite: {
          '^/api': '/api'
        }
      },
      '/ws-config': {
        target: `${serverApi}`,
        changeOrigin: true,
        pathRewrite: {
          '^/ws-config': '/ws-config'
        }
      }
    }
  }
}
