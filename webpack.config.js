const webpack = require("webpack");
const path = require('path');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const TerserJSPlugin = require('terser-webpack-plugin');
const OptimizeCSSAssetsPlugin = require('optimize-css-assets-webpack-plugin');

module.exports = () => {
    const isDevelopment = !(process.env.NODE_ENV && process.env.NODE_ENV === "production");
    return [{
        mode: isDevelopment ? "development" : "production",
        entry: './Frontend/scripts/main.ts',
        output: {
            path: path.resolve(__dirname, 'wwwroot'),
            filename: `js/[name]${isDevelopment ? "" : ".min"}.js`
        },
        module: {
            rules: [
                {
                    test: /\.js$/,
                    loader: "babel-loader?cacheDirectory=true",
                    exclude: /node_modules/
                },
                {
                    test: /\.ts(x)?$/,
                    loader: 'ts-loader',
                    exclude: /node_modules/
                },
                {
                    test: /\.css$/,
                    use: [
                        MiniCssExtractPlugin.loader,
                        "css-loader"
                    ]
                },
                {
                    test: /\.scss$/,
                    use: [
                        MiniCssExtractPlugin.loader,
                        'css-loader',
                        'sass-loader'
                    ]
                },
                {
                    test: /\.(png|jp(e)?g|gif)$/,
                    use: [
                        {
                            loader: 'url-loader?limit=10000',
                            options: { name: "images/[name].[ext]" }
                        }
                    ]
                },
            ]
        },
        resolve: {
            extensions: ['.tsx', '.ts', '.js']
        },
        plugins: [
            new webpack.ProvidePlugin({
                $: 'jquery',
                jQuery: 'jquery'
            }),
            new CleanWebpackPlugin({
                //dry: true,
                verbose: true,
                cleanOnceBeforeBuildPatterns: ['!user-images*', 'css/**/*', 'js/**/*'], 
            }),
            //new CopyWebpackPlugin({
            //    patterns: [
            //        {
            //            from: "./Frontend/favicon",
            //            to: "./",
            //            noErrorOnMissing: true,
            //        },
            //        {
            //            from: "./Frontend/images",
            //            to: "./images",
            //            noErrorOnMissing: true,
            //        }
            //    ]
            //}),
            new MiniCssExtractPlugin({
                filename: `css/[name]${isDevelopment ? "" : ".min"}.css`
            }),
        ],
        optimization: {
            minimizer: [
                new TerserJSPlugin({}),
                new OptimizeCSSAssetsPlugin({
                    cssProcessorPluginOptions: {
                        preset: [
                            "default",
                            { discardComments: { removeAll: true } }
                        ]
                    }
                })],
            splitChunks: {
                cacheGroups: {
                    vendors: {
                        test: /[\\/]node_modules[\\/]/,
                        chunks: "all",
                        name: "vendors",
                        filename: `js/[name]${isDevelopment ? "" : ".min"}.js`
                    }
                }
            }
        },
    }]
};
