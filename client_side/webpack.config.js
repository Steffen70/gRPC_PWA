const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const ReactRefreshWebpackPlugin = require("@pmmmwh/react-refresh-webpack-plugin");

module.exports = (env, argv) => {
    const isDevelopment = argv.mode !== "production";

    return {
        entry: "./src/index.jsx",
        output: {
            path: path.resolve(__dirname, "../docs"),
            filename: "bundle.js",
        },
        mode: isDevelopment ? "development" : "production",
        module: {
            rules: [
                {
                    test: /\.(js|jsx)$/,
                    exclude: /node_modules/,
                    use: {
                        loader: "babel-loader",
                        options: {
                            presets: ["@babel/preset-env", "@babel/preset-react"],
                            plugins: [
                                isDevelopment && require.resolve("react-refresh/babel")
                            ].filter(Boolean),
                        },
                    },
                },
            ],
        },
        resolve: {
            extensions: [".js", ".jsx"],
        },
        plugins: [
            new HtmlWebpackPlugin({
                template: "./src/index.html",
            }),
            isDevelopment && new ReactRefreshWebpackPlugin(),
        ].filter(Boolean),
        devServer: {
            static: {
                directory: path.join(__dirname, "../docs"),
            },
            hot: true,
            open: true,
            watchFiles: {
                paths: [path.join(__dirname, "./src/**/*")],
            },
        },
    };
};