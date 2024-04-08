const { createProxyMiddleware } = require('http-proxy-middleware');
module.exports = function (app) {
    app.use(
        '/api',
        createProxyMiddleware({
            target: 'http://178.208.76.115:5024',
            changeOrigin: true,
        })
    );
};