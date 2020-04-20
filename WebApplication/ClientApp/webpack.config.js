module.exports = {
    entry: './src/main.js',
    output: {
        path: __dirname + '/../wwwroot/js',
        filename: 'main.js'
    },
    module: {
        loaders: [
            {
                test: /\.vue$/,
                loader: 'vue-loader'
            },
            {
                test: /\.css$/,
                loader: 'style-loader!css-loader'
            }
        ]
    }
};
