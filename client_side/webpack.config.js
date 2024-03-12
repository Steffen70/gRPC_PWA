const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const ReactRefreshWebpackPlugin = require("@pmmmwh/react-refresh-webpack-plugin");

module.exports = (_env, argv) => {
    const isDevelopment = argv.mode !== "production";

    return {
        entry: "./src/index.jsx",
        output: {
            path: path.resolve(__dirname, "../docs"),
            filename: "bundle.min.js"
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
                            presets: [
                                "@babel/preset-env",
                                [
                                    "@babel/preset-react",
                                    {
                                        runtime: "automatic"
                                    }
                                ]
                            ],
                            plugins: [isDevelopment && require.resolve("react-refresh/babel")].filter(Boolean)
                        }
                    }
                },
                {
                    test: /\.css$/,
                    use: [
                        "style-loader",
                        "css-loader",
                        {
                            loader: "postcss-loader",
                            options: {
                                postcssOptions: {
                                    plugins: [require("tailwindcss"), require("autoprefixer")]
                                }
                            }
                        }
                    ]
                },
                {
                    test: /\.(ts|tsx)$/,
                    include: [path.resolve(__dirname, "src/shadcn")],
                    use: "ts-loader",
                    exclude: /node_modules/
                }
            ]
        },
        resolve: {
            extensions: [".js", ".jsx", ".ts", ".tsx"],
            alias: {
                "@": path.resolve(__dirname, "src/shadcn")
            }
        },
        plugins: [
            new HtmlWebpackPlugin({
                template: "./src/index.html"
            }),
            isDevelopment && new ReactRefreshWebpackPlugin()
        ].filter(Boolean),
        devServer: {
            static: {
                directory: path.join(__dirname, "../docs")
            },
            hot: true,
            open: true,
            watchFiles: {
                paths: [path.join(__dirname, "./src/**/*")]
            }
        }
    };
};
