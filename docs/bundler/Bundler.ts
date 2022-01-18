import {IBundlerConfig} from "./IBundlerConfig";
import * as fs from "fs";
import * as path from "path";

export class Bundler {
    public static bundle(conf: IBundlerConfig) {
        let outPaths: string[];

        if (Array.isArray(conf.outPath)) {
            outPaths = conf.outPath
        } else {
            outPaths = [conf.outPath];
        }

        let content = Bundler.combine(conf.mainFile);

        for (let outPath of outPaths) {
            console.log(`Write to ${path.join(process.cwd(), outPath, conf.outName)}`);
            fs.writeFileSync(path.join(process.cwd(), outPath, conf.outName), content);
        }
    }

    private static combine(mainFile: string): string {
        return Bundler.scan(path.join(process.cwd(), mainFile));
    }

    private static scan(p: string): string {
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

    private static insert(original: string, insert: string, index: number): string {
        return original.slice(0, index) + insert + original.slice(index);
    }

    private static cut(str: string, cutStart: number, cutEnd: number) {
        return str.substr(0, cutStart) + str.substr(cutEnd + 1);
    }
}
