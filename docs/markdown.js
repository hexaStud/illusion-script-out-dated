"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const Bundler_1 = require("./bundler/Bundler");
Bundler_1.Bundler.bundle({
    mainFile: "md/main.md",
    outName: "Readme.md",
    outPath: ["out", "../"]
});
//# sourceMappingURL=markdown.js.map