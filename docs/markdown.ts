import {Bundler} from "./bundler/Bundler";

Bundler.bundle({
    mainFile: "md/main.md",
    outName: "Readme.md",
    outPath: ["out", "../"]
});
