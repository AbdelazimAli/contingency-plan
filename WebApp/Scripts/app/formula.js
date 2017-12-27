var Checker = function () {
    /// Checker...
    /// (columns) mandatory -> else optional
    /// var args = {
    ///         columns: "{string comma saparted columns names}", 
    ///         ignoreCase: {default=true},         //ignore case when (Column List, Date Functions)
    ///         enableDateFuncs: {default=true},    //enable Today, FirstMonthInYear, ...etc
    ///         enableIdentity: {default=true}      //enable @User, ... etc
    ///    };
    /// ---Initiate Values
    /// Checker.InitialValues(args); params: (object), retuern: viod
    /// ---Check text
    /// Checker.CheckFormula(whereText, isDynamicCheck); params: (string, bool[optional]) , retuern: (object {isValid, message })

    var ColumnsStr = '', ColumnsList = [],
        ignoreCase = true,
        ID_ARGS = [], DATE_FUNC = [], //Constants
        //MathOperators = /(\+)|(-)||(\*)|(\/)|(%)/, //not used
        LogicalOperators = /(=)|(>=)|(<=)|(<)|(>)|( IN )|( StartWith )|( EndWith )|( Contains )|( LIKE )/i;

    InitialValues = function (args) {
        //set global variables
        DATE_FUNC = ['_Today', '_FirstDayInMonth', '_FirstDayInYear'];
        ID_ARGS = ['@User', '@Company', '(', ')', '@Date(', '@RoleId', '@EmpId', '@IsDepManager', '@JobId', '@LocationId', '@PositionId', '@SectorId', '@BranchId', '@DepartmentId'];
        ColumnsStr = '';
        ignoreCase = true;
        if (args) {
            if (args.enableDateFuncs === false) DATE_FUNC = [];
            if (args.enableIdentity === false) ID_ARGS = [];

            ignoreCase = args.ignoreCase === undefined ? true : false;

            ColumnsStr = args.columns || '';
            ColumnsList = ColumnsStr.split(',');

            if (ColumnsList.indexOf("Id") == -1) ColumnsList.push("Id");
        }
    }
  

    ///-----------------Main Functions-----------------
    CheckFormula = function (whereText, isDynamicCheck) {
        var resultObject = { isValid: true, message: null };
        var processedString;
        var firstClosedBracketIndex = whereText.indexOf(')');
        var openBracketIndex = SearchOpenBracketIndexBeforeSpecificIndex(whereText, firstClosedBracketIndex, '(');

        if (whereText == '') return { isValid: true, message: null };

        if (firstClosedBracketIndex == -1 && openBracketIndex == -1) {      // It is clean from brackets                        
            return CheckGeneralEquation(whereText);
        }
        else if (firstClosedBracketIndex == -1 && openBracketIndex != -1) {
            return { isValid: false, message: "There is open without closed bracket " };
        }
        else if (firstClosedBracketIndex != -1 && openBracketIndex == -1) {
            return { isValid: false, message: "there is a closed bracket but not opened" };
        }
        else {      // there is opened and closed bracket
            var equation = whereText.substring(openBracketIndex + 1, firstClosedBracketIndex);
            var replacementChar = '#';

            if (equation.indexOf(',') != -1) { //Check if 'in' Clause
                resultObject = CheckInClose(equation);
                if (resultObject.isValid)
                    replacementChar = '#in';
            }
            else {  //Not 'in' Clause
                resultObject = CheckGeneralEquation(equation, true);
                replacementChar = '#';
            }

            //if valid => recursive
            if (resultObject.isValid) {
                processedString = ReplaceEquationWitFlagChar(whereText, replacementChar, openBracketIndex, firstClosedBracketIndex);
                return CheckFormula(processedString, isDynamicCheck);
            }
        }

        return resultObject;
    }

    //Check And/Or --- Flag for empty statement
    CheckGeneralEquation = function (whereText, flag) {
        var flag = true, resultObject = { isValid: true, message: null };
        var arr = SplitBaseonOrAnd(whereText);
        
        if (arr.length == 1) { //array doesn't contain any or/and
            
            if (arr[0] != '#') 
                resultObject = CheckEquation(arr[0]);
             else if (flag) 
                resultObject = { isValid: true, message: null };
        
        } else {
            for (var i = 0; i < arr.length; i++) {
                if (/^(and)|(or)$/i.test(arr[i])) { //arr[i] == 'and' || arr[i] == 'or'

                    if (arr[i - 1] != '#') {  //Take left hand side 
                        flag = false;
                        resultObject = CheckEquation(arr[i - 1]);
                        if (resultObject.isValid == false)
                            break;
                    }
                    if (arr[i + 1] != '#') {  //Take the right hand side part
                        flag = false;
                        resultObject = CheckEquation(arr[i + 1]);
                        if (resultObject.isValid == false)
                            break;
                    }

                }
            }
            if (flag) resultObject = { isValid: true, message: null };
        }
        return resultObject;                                                                                                                
    }

    //Check left side and right side
    CheckEquation = function (whereText) {
        var result, output = { isValid: true, message: null };

        var arr = SplitEquation(whereText, LogicalOperators);
        var length = arr.length;
        
        if (length == 1) {
            if (isEmptyOrWhitespace(arr[0])) {
                result = "Invalid empty equation after and/or";
            }
            else {
                result = "No operator found after ";
                if (arr[0].charAt(arr[0].length - 1) == '#') {
                    var strWithoutHash = arr[0].substring(0, arr[0].length - 1);
                    result = result + strWithoutHash;
                } else {
                    result = result + arr[0];
                }
            }
            output.isValid = false;
        }
        else if (length > 3) {
            result = "Invalid operator in equation " + whereText;
            output.isValid = false;
        }
        else {
            var leftOperand = arr[0], operator = arr[1], rightOperand = arr[2];

            leftOperand = leftOperand.trim(); //.replace(/\s+/g, '');
            rightOperand = rightOperand == undefined ? rightOperand : rightOperand.trim(); //.replace(/\s+/g, '');

            if (SearchList(leftOperand, ColumnsList, ignoreCase) == -1) { //If Left Operator not in ColumnList
                if (leftOperand.charAt(0) == '#') 
                    result = "Expected And/Or required before " + leftOperand.substring(1, leftOperand.length + 1); //leftOperand Without Hash
                 else 
                    result = "Invalid column name " + leftOperand;
                
                output.isValid = false;
            }
            else if (rightOperand != '') { ///Right Operator Accept only (Numbers, True/False, DateFunc)
                if (/( in )/i.test(operator)) {
                    if (rightOperand != "#in") {
                        output.isValid = false;
                        result = "Expected ( required after 'in'";
                    }
                }
                else if (SearchList(rightOperand, ID_ARGS, false) == -1 && SearchList(rightOperand, DATE_FUNC, ignoreCase) == -1)  //If not in Identity Arguments or Date Function
                {
                    if (isNaN(Number(rightOperand))) {
                        if (rightOperand.lastIndexOf("\"") != -1) { //is date or srting
                            var res = IsDateOrStr(rightOperand, leftOperand);
                            output.isValid = res.isValid;
                            result = res.message;
                        }
                        else if (!stringToBool(rightOperand)) {
                            if (rightOperand == '#')
                                result = "parameter cannot be empty";
                            else if (rightOperand.charAt(rightOperand.length - 1) == '#')
                                result = "Expected And/OR required after " + rightOperand.replace('#', '');
                            else
                                result = "Invalid parameter " + rightOperand;

                            output.isValid = false;
                        }
                    }
                }
            }
            else {
                result = "parameter cannot be empty after " + leftOperand + " " + operator;
                output.isValid = false;
            }
        }
        output.message = result;
        return output;
    }


    ///-----------------Helper Checkes-----------------
    //Check if date or string is valid
    IsDateOrStr = function (rightOperand, leftOperand) {
        var closeQuote = rightOperand.lastIndexOf("\"");
        if (closeQuote != -1) {
            if (SearchOpenBracketIndexBeforeSpecificIndex(rightOperand, closeQuote, "\"") == -1)
                return { isValid: false, message: "dates and strings must be between single quots" };
            else {
                //check date formate
                if (leftOperand.indexOf('Date') != -1 && !/^"[0-9]{4}\-[0-9]{2}\-[0-9]{2}"/.test(rightOperand))
                    return { isValid: false, message: "dates must be in 'yyyy-MM-dd' formate" };
            }
        }
        return { isValid: true, message: null };
    }

    //Chick if 'in' close '()' is valid
    CheckInClose = function (rightOperand) {

        var inClose = rightOperand.split(',');
        if (inClose == '')
            return { isValid: false, message: "'in' barckets cannot be empty" };

        for (var i = 0; i < inClose.length; i++) {
            inClose[i] = inClose[i].trim();

            if (isNaN(Number(inClose[i])))
                return { isValid: false, message: "invalid integer after 'in' " };
            else if (inClose[i] == '')
                return { isValid: false, message: "invalid integer after comma 'in' " };
        }

        return { isValid: true, message: null };
    }


    ///-----------------Spliters-----------------
    SplitEquation = function (equation, splitter) {
        var arr = equation.split(splitter);

        arr = arr.filter(function (element) {
            return element !== undefined;
        });

        return arr;
    }

    SplitBaseonOrAnd = function (str) {
        var arr = str.split(/(\s+and\s?)|(\s+or\s?)/i);

        arr = arr.filter(function (element) {
            return element !== undefined;
        });

        arr = arr.map(function (str) {
            return str.trim();
        });

        return arr;
    }


    ///-----------------Helper Functions-----------------
    String.prototype.splice = function (idx, rem, str) {
        if (this && typeof(this) == "string") return this.slice(0, idx) + str + this.slice(idx + Math.abs(rem));
    }

    stringToBool = function (str) {
        var result = false;
        if (str.match(/^(true|false)$/i) !== null)
            result = true;

        return result;
    }

    isEmptyOrWhitespace = function (str) {
        return str === null || str.match(/^\s*$/) !== null;
    }

    SearchList = function (key, list, ignoreCase) {
        var result = -1;
        if (list != null) {
            if (ignoreCase) 
                result = list.findIndex(i => i.toLowerCase() == key.toLowerCase());
             else 
                result = list.indexOf(key);
        }
        return result;
    }

    SearchColumnList = function (column) {
        return ColumnsList.indexOf(column);
    }

    SearchOpenBracketIndexBeforeSpecificIndex = function (str, index, bracketChar) {
        var result = -1;
        var partBeforeIndex = str.substring(0, index);

        for (var i = index - 1; i >= 0; i--) {
            if (partBeforeIndex.charAt(i) === bracketChar) {
                result = i;
                break;
            }
        }

        return result;
    }

    ReplaceEquationWitFlagChar = function (str, flag, start, end) {
        var currentStr = str.substring(start, end + 1);
        str = str.replace(currentStr, flag);
        return str;
    }


    ///------------------------not used---------------------------------------
    //-------math and logic-------
    //(x,y,z), split(+-..<=..)
    CheckFormulaLogical = function (whereText, isDynamicCheck) {
        var resultObject;
        var processedString;
        var firstClosedBracketIndex = whereText.indexOf(')');

        if (firstClosedBracketIndex == -1) {
            if (whereText.indexOf('(') != -1) {
                resultObject = { isValid: false, message: "There is open without closed bracket " }

            } else {
                // It is clean from brackets                        
                resultObject = CheckLogicalEquation(whereText);
            }
        } else {
            var openBracketIndex = SearchOpenBracketIndexBeforeSpecificIndex(whereText, firstClosedBracketIndex, '(');
            if (openBracketIndex == -1) {
                resultObject = { isValid: false, message: "there is a closed bracket but not opened" };

            } else {
                // there is opened and closed bracket
                var equation = whereText.substring(openBracketIndex + 1, firstClosedBracketIndex);

                resultObject = CheckLogicalEquation(equation, true);

                //if not (resultObject == null && isDynamicCheck != true) {
                if (resultObject.isValid) {
                    processedString = ReplaceEquationWitFlagChar(whereText, '#', openBracketIndex, firstClosedBracketIndex);
                    return CheckFormulaLogical(processedString, isDynamicCheck);
                }

            }
        }

        return resultObject;
    }

    // Flag for empty statement
    CheckLogicalEquation = function (whereText, flag) {
        var flag = true, resultObject = { isValid: true, message: null };

        var arr = SplitBaseonOrAnd(whereText);
        if (arr.length == 1) {
            if (arr[0] != '#')
                resultObject = FilterEquation(arr[0]);
            else if (flag)
                resultObject = { isValid: true, message: null };

        } else {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i] == 'and' || arr[i] == 'or') {

                    if (arr[i - 1] != '#') {  // Take left hand side 
                        flag = false;
                        resultObject = FilterEquation(arr[i - 1]);
                        if (resultObject.isValid == false)
                            break;
                    }
                    if (arr[i + 1] != '#') {   // Take the right hand side part
                        flag = false;
                        resultObject = FilterEquation(arr[i + 1]);
                        if (resultObject.isValid == false)
                            break;
                    }
                }
            }
            if (flag) resultObject = { isValid: true, message: null };
        }
        return resultObject;
    }

    FilterEquation = function (equation) {
        var resultObject, arr = SplitEquation(equation, /(<)|(>)|(<=)|(>=)|(=)/);
        var length = arr.length;
        
        if (length == 1) {
            resultObject = CheckEquationForLogical(arr[0]);

        } else if (length > 3) {
            resultObject = { isValid: false, message: "Invalid equation '" + equation + "' " };
        } else {
            resultObject = CheckEquationForLogical(arr[0]);

            if (resultObject.isValid)
                resultObject = CheckEquationForLogical(arr[2])
        }
        return resultObject;
    }

    //Check left side and right side
    CheckEquationForLogical = function (whereText) {
        var arrColumnList = ['x', 'y', 'z'];

        var result, output = { isValid: true, message: null };

        var arr = SplitEquationToArrLogical(whereText);
        var length = arr.length;

        if (length == 0) {
            output.isValid = false;
            result = "Invalid empty equation after and/or";
        }
        else if (length == 1) {
            output.isValid = false;

            if (isEmptyOrWhitespace(arr[0])) {
                result = "Invalid empty equation after and/or";
            }
            else if (isNaN(Number(arr[0]))) {
                // is it found in column list 
                if (SearchList(arr[0], arrColumnList) == -1)
                    result = "Invalid parameter '" + arr[0] + "'";
                else
                    output.isValid = true;
            }

        }
        else if (length > 3) {
            result = "Invalid operator in equation " + whereText;
            output.isValid = false;
        }
        else {
            var leftOperand = arr[0], operator = arr[1], rightOperand = arr[2];

            leftOperand = leftOperand.replace(/\s+/g, '');
            rightOperand = rightOperand.replace(/\s+/g, '');

            if (SearchList(leftOperand, arrColumnList) == -1) {
                if (isNaN(Number(leftOperand))) {
                    if (leftOperand.charAt(0) == '#') {
                        var leftOperandWithoutHash = leftOperand.substring(1, leftOperand.length + 1);
                        result = "Expected And/Or required before " + leftOperandWithoutHash;
                    } else {
                        result = "'" + leftOperand + "' is invalid column name or integer";
                    }
                    output.isValid = false;
                }
            }
            else if (rightOperand != '') {
                if (SearchList(rightOperand, arrColumnList) == -1) {
                    if (isNaN(Number(rightOperand))) {

                        if (rightOperand == '#') {
                            result = "parameter cannot be empty";
                        }
                        else if (rightOperand.charAt(rightOperand.length - 1) == '#') {
                            var operandWithoutHash = rightOperand.replace('#', '');
                            result = "Expected And/OR required after " + operandWithoutHash;
                        } else {
                            result = "'" + rightOperand + "' is invalid column name or integer";
                        }
                        output.isValid = false;
                    }
                }
            }
            else { //left and rigth == ""
                result = "parameter cannot be empty after " + leftOperand + " " + operator;
                output.isValid = false;
            }
        }
        output.message = result;
        return output;
    }

    //spliter
    SplitEquationToArrLogical = function (equation) {
        var arr = equation.split(/(=)|(>=)|(<=)|(<)|(>)|(\+)|(-)||(\*)|(\/)|(%)/);

        arr = arr.filter(function (element) {
            return element !== undefined;
        });
        
        arr = arr.filter(function (element) {
            return /\S+/.test(element);
        });

        return arr;
    }
    //----------------------

    //GetStringFromArray = function (arr) {
    //    var result;
    //    for (var i = 0; i < arr.length; i++) {
    //        if (i == 0)
    //            result = arr[i];
    //        else
    //            result += ", " + arr[i];
    //    }
    //    return result;
    //}
    //SplitEquationBasedOnParentheses = function (equation) {
    //    var arr = equation.split(/(\()|(\))/);
    //    arr = arr.filter(function (element) {
    //        return element !== undefined;
    //    });
    //    return arr;
    //}
    //isvalidEquation = function (str) {
    //    var arr = SplitEquation(whereText, /(=)|(>=)|(<=)|(<)|(>)/);
    //    return (arr.length == 3);
    //}


    ///------------------------end not used---------------------------------------

    return {
        InitialValues: InitialValues,
        isEmptyOrWhitespace: isEmptyOrWhitespace,
        CheckFormula: CheckFormula,
        CheckInClose: CheckInClose,
        stringToBool: stringToBool
    }
}();