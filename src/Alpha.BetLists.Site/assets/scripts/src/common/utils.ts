namespace Common {
    export class Utils {
        public static IsNullOrUndefinedOrEmpty = (value: any): boolean => value === null || value === undefined || value === '';
    }
}