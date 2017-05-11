namespace Common {
    export class Utils {
        public static IsNullOrUndefinedOrEmpty = (value: any): boolean => value === null || value === undefined || value === '';

        public static ParseBoolean = (value: string): boolean => {
            if (!Utils.IsNullOrUndefinedOrEmpty(value)) {
                return value.toLowerCase() === "true";
            }

            return false;
        }
    }
}