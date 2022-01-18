"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Bundler = void 0;
const fs = require("fs");
const path = require("path");
class Bundler {
    static bundle(conf) {
        let outPaths;
        if (Array.isArray(conf.outPath)) {
            outPaths = conf.outPath;
        }
        else {
            outPaths = [conf.outPath];
        }
        let content = Bundler.combine(conf.mainFile);
        for (let outPath of outPaths) {
            console.log(`Write to ${path.join(process.cwd(), outPath, conf.outName)}`);
            fs.writeFileSync(path.join(process.cwd(), outPath, conf.outName), content);
        }
    }
    static combine(mainFile) {
        return Bundler.scan(path.join(process.cwd(), mainFile));
    }
    static scan(p) {
        console.log(`Scan ${p}`);
        let content = fs.readFileSync(p, "utf8");
        let searchIndex = 0;
        while ((searchIndex = content.search(/^\[comment]: <> \(#..*\)$/gm)) !== -1) {
            let endPos = content.indexOf(")", searchIndex);
            let imp = content.substring(searchIndex, endPos);
            let file = imp.substring(16, imp.length);
            content = Bundler.cut(content, searchIndex, endPos);
            console.log(`Import ${path.join(path.dirname(p))}`);
            let impContent = Bundler.scan(path.join(path.dirname(p), file));
            content = Bundler.insert(content, impContent, searchIndex);
        }
        return content;
    }
    static insert(original, insert, index) {
        return original.slice(0, index) + insert + original.slice(index);
    }
    static cut(str, cutStart, cutEnd) {
        return str.substr(0, cutStart) + str.substr(cutEnd + 1);
    }
}
exports.Bundler = Bundler;
//# sourceMappingURL=Bundler.js.map