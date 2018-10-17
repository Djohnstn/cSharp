using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIMCollect.SqlClasses
{
    // this is a dictionary of all word phrases in the stored procedures as of last year, 
    // so it'll compress a lot of words down to 1, 2, or 3 characters of text, giving great compression at high speed.
    public static class SqlWordDictionary
    {
        public static bool TryGetHash(string key, out string value)
        {

            if (WordHashBase.TryGetValue(key, out string value2))
            {
                value = value2;
                return true;
            }
            else
            {
                value = string.Empty;
                return false;
            }
        }

        public static readonly Dictionary<string, string> WordHashBase = new Dictionary<string, string>
        {
            {"=", "A"},  // Count: 6153, Ordinal: 0
            {";", "B"},  // Count: 5616, Ordinal: 1
            {"@PARAMETERLIST", "C"},  // Count: 4727, Ordinal: 2
            {"(", "D"},  // Count: 4608, Ordinal: 3
            {")", "E"},  // Count: 4608, Ordinal: 4
            {"+", "F"},  // Count: 3246, Ordinal: 5
            {"SET", "G"},  // Count: 2957, Ordinal: 6
            {".", "H"},  // Count: 2856, Ordinal: 7
            {"BEGIN", "I"},  // Count: 1996, Ordinal: 8
            {"END", "J"},  // Count: 1972, Ordinal: 9
            {"NULL", "K"},  // Count: 1357, Ordinal: 10
            {"IF", "L"},  // Count: 1247, Ordinal: 11
            {"AS", "M"},  // Count: 1219, Ordinal: 12
            {"NCHAR", "AA"},  // Count: 1098, Ordinal: 13
            {"NVARCHAR", "BA"},  // Count: 1098, Ordinal: 14
            {"ELSE", "CA"},  // Count: 917, Ordinal: 15
            {"DBO", "DA"},  // Count: 881, Ordinal: 16
            {"IS", "EA"},  // Count: 833, Ordinal: 17
            {"INT", "FA"},  // Count: 759, Ordinal: 18
            {"N'//'", "GA"},  // Count: 757, Ordinal: 19
            {"WHERE", "IA"},  // Count: 730, Ordinal: 21
            {"CREATE", "JA"},  // Count: 707, Ordinal: 22
            {"PROCEDURE", "KA"},  // Count: 703, Ordinal: 23
            {"AND", "LA"},  // Count: 682, Ordinal: 24
            {"RULES", "MA"},  // Count: 648, Ordinal: 25
            {"FROM", "AB"},  // Count: 617, Ordinal: 26
            {"SELECT", "BB"},  // Count: 534, Ordinal: 27
            {"@@ERROR", "CB"},  // Count: 509, Ordinal: 28
            {"RETURN", "DB"},  // Count: 439, Ordinal: 29
            {"DECLARE", "EB"},  // Count: 409, Ordinal: 30
            {"@SAVERR", "HB"},  // Count: 371, Ordinal: 33
            {"RULEID", "IB"},  // Count: 326, Ordinal: 34
            {"<>", "JB"},  // Count: 314, Ordinal: 35
            {"CITATIONS", "KB"},  // Count: 243, Ordinal: 36
            {"INTO", "LB"},  // Count: 237, Ordinal: 37
            {"ON", "MB"},  // Count: 215, Ordinal: 38
            {"BIT", "BC"},  // Count: 208, Ordinal: 40
            {"GROUP_NAME", "CC"},  // Count: 208, Ordinal: 41
            {"INSERT", "DC"},  // Count: 201, Ordinal: 42
            {"JOIN", "FC"},  // Count: 196, Ordinal: 44
            {"VALUES", "GC"},  // Count: 193, Ordinal: 45
            {"DATETIME", "HC"},  // Count: 188, Ordinal: 46
            {"@ERRORSAVE", "IC"},  // Count: 182, Ordinal: 47
            {"LIST_POINTS", "JC"},  // Count: 180, Ordinal: 48
            {"DELETE", "KC"},  // Count: 177, Ordinal: 49
            {"UPDATE", "LC"},  // Count: 173, Ordinal: 50
            {"STRATEGYID", "MC"},  // Count: 167, Ordinal: 51
            {"@STRATEGYID", "AD"},  // Count: 165, Ordinal: 52
            {"DESCRIPTION", "BD"},  // Count: 164, Ordinal: 53
            {"@RULEID", "CD"},  // Count: 155, Ordinal: 54
            {"OR", "DD"},  // Count: 153, Ordinal: 55
            {"CC", "ED"},  // Count: 147, Ordinal: 56
            {"AREAID", "HD"},  // Count: 122, Ordinal: 59
            {"@QUERYSTRING", "ID"},  // Count: 121, Ordinal: 60
            {"BY", "JD"},  // Count: 121, Ordinal: 61
            {"*", "KD"},  // Count: 119, Ordinal: 62
            {"ORDER", "LD"},  // Count: 118, Ordinal: 63
            {"@VCOUNT", "MD"},  // Count: 111, Ordinal: 64
            {"EXEC", "AE"},  // Count: 110, Ordinal: 65
            {"CITATIONID", "BE"},  // Count: 108, Ordinal: 66
            {"TRANSACTION", "CE"},  // Count: 105, Ordinal: 67
            {"100", "DE"},  // Count: 104, Ordinal: 68
            {"RTRIM", "EE"},  // Count: 103, Ordinal: 69
            {"TAGNAME", "FE"},  // Count: 103, Ordinal: 70
            {"SORT", "GE"},  // Count: 102, Ordinal: 71
            {"LIST_AREA_NAME", "HE"},  // Count: 101, Ordinal: 72
            {"TIMESTAMP", "IE"},  // Count: 98, Ordinal: 73
            {"SUPPLEMENTALDATA", "JE"},  // Count: 93, Ordinal: 74
            {"@TAGNAME", "LE"},  // Count: 92, Ordinal: 76
            {"AREAS", "ME"},  // Count: 92, Ordinal: 77
            {"TAG_NAME", "BF"},  // Count: 87, Ordinal: 79
            {"CSRULES", "DF"},  // Count: 85, Ordinal: 81
            {"ACTIVE", "FF"},  // Count: 82, Ordinal: 83
            {"@BATCH", "GF"},  // Count: 79, Ordinal: 84
            {"MAILBOXID", "HF"},  // Count: 78, Ordinal: 85
            {"STRATEGIES", "IF"},  // Count: 78, Ordinal: 86
            {"CUR", "JF"},  // Count: 77, Ordinal: 87
            {"CLOSINGLOGID", "KF"},  // Count: 76, Ordinal: 88
            {"@CAUSEID", "LF"},  // Count: 75, Ordinal: 89
            {"@MATERIAL", "MF"},  // Count: 72, Ordinal: 90
            {"TAGS", "AG"},  // Count: 71, Ordinal: 91
            {"@GROUP", "CG"},  // Count: 68, Ordinal: 93
            {"LIST", "DG"},  // Count: 67, Ordinal: 94
            {"@ACTIVE", "EG"},  // Count: 66, Ordinal: 95
            {">", "FG"},  // Count: 66, Ordinal: 96
            {"ALARMSTATUS", "GG"},  // Count: 66, Ordinal: 97
            {"CAUSEID", "HG"},  // Count: 66, Ordinal: 98
            {"@DESCRIPTION", "IG"},  // Count: 65, Ordinal: 99
            {"@PLANT_ID", "JG"},  // Count: 65, Ordinal: 100
            {"@TYPE", "KG"},  // Count: 65, Ordinal: 101
            {"DISTINCT", "LG"},  // Count: 65, Ordinal: 102
            {"N'", "MG"},  // Count: 65, Ordinal: 103
            {"SUPPLEMENTALACTION", "AH"},  // Count: 65, Ordinal: 104
            {"ACTIONID", "BH"},  // Count: 64, Ordinal: 105
            {"LIST_NAME", "CH"},  // Count: 64, Ordinal: 106
            {"@ENDTIME", "DH"},  // Count: 63, Ordinal: 107
            {"@RESOURCENETWORK", "EH"},  // Count: 62, Ordinal: 108
            {"LEFT", "FH"},  // Count: 62, Ordinal: 109
            {"@STORAGE_LOCATION", "GH"},  // Count: 61, Ordinal: 110
            {"ACTIONS", "HH"},  // Count: 61, Ordinal: 111
            {"COUNT", "IH"},  // Count: 61, Ordinal: 112
            {"RDMSERVER", "JH"},  // Count: 61, Ordinal: 113
            {"'", "KH"},  // Count: 60, Ordinal: 114
            {"PRODUCTION_UNIT_NAME", "LH"},  // Count: 60, Ordinal: 115
            {"INNER", "MH"},  // Count: 59, Ordinal: 116
            {"<", "AI"},  // Count: 58, Ordinal: 117
            {"FETCH", "BI"},  // Count: 58, Ordinal: 118
            {"NEXT", "CI"},  // Count: 58, Ordinal: 119
            {"@EVENT_DATE", "EI"},  // Count: 57, Ordinal: 121
            {"@GMN", "FI"},  // Count: 57, Ordinal: 122
            {"@RECIPE_ID", "GI"},  // Count: 57, Ordinal: 123
            {"@EVENT_TIME", "JI"},  // Count: 55, Ordinal: 126
            {"ERRORHANDLER:", "KI"},  // Count: 55, Ordinal: 127
            {"SEQUENCE", "LI"},  // Count: 55, Ordinal: 128
            {"@AREA", "MI"},  // Count: 54, Ordinal: 129
            {"@INSSTAT", "AJ"},  // Count: 54, Ordinal: 130
            {"@UNIT_OF_MEASURE", "BJ"},  // Count: 54, Ordinal: 131
            {"CITLOG", "CJ"},  // Count: 54, Ordinal: 132
            {"@GROUP_NAME", "DJ"},  // Count: 53, Ordinal: 133
            {"@PROCESS_ORDER", "EJ"},  // Count: 53, Ordinal: 134
            {"@SORT", "FJ"},  // Count: 53, Ordinal: 135
            {"CONTINUOUSALARM", "GJ"},  // Count: 53, Ordinal: 136
            {"@HNDL", "HJ"},  // Count: 52, Ordinal: 137
            {"@MAILBOXID", "IJ"},  // Count: 52, Ordinal: 138
            {"@STARTTIME", "JJ"},  // Count: 52, Ordinal: 139
            {"TBL2", "KJ"},  // Count: 52, Ordinal: 140
            {"@AREAID", "MJ"},  // Count: 51, Ordinal: 142
            {"@TRACEID", "AK"},  // Count: 51, Ordinal: 143
            {"@ACTIONID", "BK"},  // Count: 50, Ordinal: 144
            {"CSCRITERIA", "CK"},  // Count: 50, Ordinal: 145
            {"OUTPUT", "DK"},  // Count: 50, Ordinal: 146
            {"@BATCHID", "EK"},  // Count: 48, Ordinal: 147
            {"@COLUMNID", "FK"},  // Count: 48, Ordinal: 148
            {"@MSCLA", "GK"},  // Count: 48, Ordinal: 149
            {"@ZERODATE", "HK"},  // Count: 48, Ordinal: 150
            {"REAL", "IK"},  // Count: 48, Ordinal: 151
            {"@ONBIT", "JK"},  // Count: 47, Ordinal: 152
            {"@SAVEERR", "KK"},  // Count: 47, Ordinal: 153
            {"C", "LK"},  // Count: 47, Ordinal: 154
            {"CLOSE", "MK"},  // Count: 47, Ordinal: 155
            {"AREANAME", "AL"},  // Count: 46, Ordinal: 156
            {"DEALLOCATE", "BL"},  // Count: 46, Ordinal: 157
            {"N''", "CL"},  // Count: 46, Ordinal: 158
            {"@EVENTID", "DL"},  // Count: 45, Ordinal: 159
            {"@LISTAREA", "EL"},  // Count: 45, Ordinal: 160
            {"@NUM", "FL"},  // Count: 45, Ordinal: 161
            {"@ON", "GL"},  // Count: 45, Ordinal: 162
            {"CSCITATIONCRITERIA", "HL"},  // Count: 45, Ordinal: 163
            {"MAIL", "IL"},  // Count: 45, Ordinal: 164
            {"PRNT", "JL"},  // Count: 45, Ordinal: 165
            {"RID", "KL"},  // Count: 45, Ordinal: 166
            {"SP_TRACE_SETEVENT", "LL"},  // Count: 45, Ordinal: 167
            {"TBL1", "ML"},  // Count: 45, Ordinal: 168
            {"COMPLEXALARMS", "AM"},  // Count: 44, Ordinal: 169
            {"GMN", "BM"},  // Count: 44, Ordinal: 170
            {"@LIST_AREA_NAME_2", "CM"},  // Count: 43, Ordinal: 171
            {"AREA", "DM"},  // Count: 43, Ordinal: 172
            {"CSSTRATEGIES", "EM"},  // Count: 43, Ordinal: 173
            {"CSSUPPLEMENTALDATA", "FM"},  // Count: 43, Ordinal: 174
            {"ROLLBACK", "GM"},  // Count: 43, Ordinal: 175
            {"USEMAILBOX", "HM"},  // Count: 43, Ordinal: 176
            {"EVALUATIONDELAY", "IM"},  // Count: 42, Ordinal: 177
            {"GENERATECITATION", "JM"},  // Count: 42, Ordinal: 178
            {"OPERATOR", "KM"},  // Count: 42, Ordinal: 179
            {"PERSISTALARM", "LM"},  // Count: 42, Ordinal: 180
            {"ACTIONSCURSOR", "MM"},  // Count: 40, Ordinal: 181
            {"ATTEMPTS", "AN"},  // Count: 40, Ordinal: 182
            {"FOR", "BN"},  // Count: 40, Ordinal: 183
            {"PIMSPENDINGRLINKTRANSACTIONS", "CN"},  // Count: 40, Ordinal: 184
            {"@ALARMSTATUS", "DN"},  // Count: 39, Ordinal: 185
            {"CONVERT", "EN"},  // Count: 39, Ordinal: 186
            {"CSCAUSERELATIONS", "FN"},  // Count: 39, Ordinal: 187
            {"GETDATE", "GN"},  // Count: 39, Ordinal: 188
            {"@RDMSERVER_1", "IN"},  // Count: 37, Ordinal: 190
            {"PARENTCAUSEID", "JN"},  // Count: 37, Ordinal: 191
            {"PRINT", "KN"},  // Count: 37, Ordinal: 192
            {"VALUE", "LN"},  // Count: 37, Ordinal: 193
            {"2000", "MN"},  // Count: 36, Ordinal: 194
            {"@GROUP_NAME_3", "AO"},  // Count: 36, Ordinal: 195
            {"@USEMAILBOX", "BO"},  // Count: 36, Ordinal: 196
            {"CSSTRATEGYCAUSES", "CO"},  // Count: 36, Ordinal: 197
            {"LIST_AREA", "DO"},  // Count: 36, Ordinal: 198
            {"N'Y'", "EO"},  // Count: 36, Ordinal: 199
            {"MESSAGETYPE", "GO"},  // Count: 35, Ordinal: 201
            {"PARAMETERLIST", "HO"},  // Count: 35, Ordinal: 202
            {"RESOURCE_NETWORK", "IO"},  // Count: 35, Ordinal: 203
            {"STOREDPROCEDURE", "JO"},  // Count: 35, Ordinal: 204
            {"@GENERATECITATION", "KO"},  // Count: 34, Ordinal: 205
            {"@PERSISTALARM", "LO"},  // Count: 34, Ordinal: 206
            {"BATCH_ID", "MO"},  // Count: 34, Ordinal: 207
            {"NOT", "AP"},  // Count: 34, Ordinal: 208
            {"PRODUCTION_UNIT", "BP"},  // Count: 34, Ordinal: 209
            {"RDMGRPNAME", "CP"},  // Count: 34, Ordinal: 210
            {"@PRODUCTION_UNIT_NAME", "DP"},  // Count: 33, Ordinal: 211
            {"CSAREAS", "EP"},  // Count: 33, Ordinal: 212
            {"TYPE", "FP"},  // Count: 33, Ordinal: 213
            {"@EVALUATIONDELAY", "GP"},  // Count: 32, Ordinal: 214
            {"@LASTALARM", "HP"},  // Count: 32, Ordinal: 215
            {"@LIST", "IP"},  // Count: 32, Ordinal: 216
            {"@SUPPLEMENTALACTION", "JP"},  // Count: 32, Ordinal: 217
            {"CSCITATIONS", "KP"},  // Count: 32, Ordinal: 218
            {"LASTALARM", "LP"},  // Count: 32, Ordinal: 219
            {"SAMPLEIDTAGNAME", "MP"},  // Count: 32, Ordinal: 220
            {"COMMIT", "AQ"},  // Count: 31, Ordinal: 221
            {"FLOAT", "BQ"},  // Count: 31, Ordinal: 222
            {"HISTORICALALARM", "CQ"},  // Count: 31, Ordinal: 223
            {"PRIORITY", "DQ"},  // Count: 31, Ordinal: 224
            {"@@FETCH_STATUS", "EQ"},  // Count: 30, Ordinal: 225
            {"@@IDENTITY", "FQ"},  // Count: 30, Ordinal: 226
            {"@GROUP_3", "GQ"},  // Count: 30, Ordinal: 227
            {"@GROUP_NAME_2", "HQ"},  // Count: 30, Ordinal: 228
            {"@ROWCNT", "IQ"},  // Count: 30, Ordinal: 229
            {"@STATUS", "JQ"},  // Count: 30, Ordinal: 230
            {"CRITERIATYPEID", "KQ"},  // Count: 30, Ordinal: 231
            {"DATECREATED", "LQ"},  // Count: 30, Ordinal: 232
            {"HISTALARMMAILID", "MQ"},  // Count: 30, Ordinal: 233
            {"OPERATORID", "AR"},  // Count: 30, Ordinal: 234
            {"PIMSCHARTLINK", "BR"},  // Count: 30, Ordinal: 235
            {"ROLENAME", "CR"},  // Count: 30, Ordinal: 236
            {"STRATEGYNAME", "DR"},  // Count: 30, Ordinal: 237
            {"SUPPLEMENTALDATATYPE", "ER"},  // Count: 30, Ordinal: 238
            {"@GROUPNAME_1", "FR"},  // Count: 29, Ordinal: 239
            {"CURSOR", "GR"},  // Count: 29, Ordinal: 240
            {"HISTLIMITS", "HR"},  // Count: 29, Ordinal: 241
            {"MAILBOXNAME", "IR"},  // Count: 29, Ordinal: 242
            {"OPEN", "JR"},  // Count: 29, Ordinal: 243
            {"WHILE", "KR"},  // Count: 29, Ordinal: 244
            {"@COMPLEXALARMS", "LR"},  // Count: 28, Ordinal: 245
            {"DESC", "MR"},  // Count: 28, Ordinal: 246
            {"GROUPNAME", "AS"},  // Count: 28, Ordinal: 247
            {"LIKE", "BS"},  // Count: 27, Ordinal: 248
            {"OF", "CS"},  // Count: 27, Ordinal: 249
            {"R", "DS"},  // Count: 27, Ordinal: 250
            {"4000", "ES"},  // Count: 26, Ordinal: 251
            {"@DATECREATED", "FS"},  // Count: 26, Ordinal: 252
            {"@DEADTIME", "GS"},  // Count: 26, Ordinal: 253
            {"@GRADE", "HS"},  // Count: 26, Ordinal: 254
            {"@PRODUCTION_UNIT_NAME_1", "IS"},  // Count: 26, Ordinal: 255
            {"CHILDCAUSEID", "JS"},  // Count: 26, Ordinal: 256
            {"FULL_PATH", "KS"},  // Count: 26, Ordinal: 257
            {"LEN", "LS"},  // Count: 26, Ordinal: 258
            {"@CITATIONID", "MS"},  // Count: 25, Ordinal: 259
            {"INPUT", "AT"},  // Count: 25, Ordinal: 260
            {"PIMSADMINSUBLEVEL", "BT"},  // Count: 25, Ordinal: 261
            {"RDMGROUP", "CT"},  // Count: 25, Ordinal: 262
            {"@CALCULATIONSETNAME_1", "DT"},  // Count: 24, Ordinal: 263
            {"@COMMENTTAG", "ET"},  // Count: 24, Ordinal: 264
            {"@CONTINUOUSALARM", "FT"},  // Count: 24, Ordinal: 265
            {"@MATERIAL_CONSUMED", "GT"},  // Count: 24, Ordinal: 266
            {"@OPERATOR", "HT"},  // Count: 24, Ordinal: 267
            {"@PRODUCT", "IT"},  // Count: 24, Ordinal: 268
            {"@ROLENAME_1", "JT"},  // Count: 24, Ordinal: 269
            {"@SUPPLEMENTALDATA", "KT"},  // Count: 24, Ordinal: 270
            {"CALCULATIONSETNAME", "LT"},  // Count: 24, Ordinal: 271
            {"CHARTLEVELDATA", "MT"},  // Count: 24, Ordinal: 272
            {"CRITERIA", "AU"},  // Count: 24, Ordinal: 273
            {"EXISTS", "BU"},  // Count: 24, Ordinal: 274
            {"PRODUCT_NAME", "CU"},  // Count: 24, Ordinal: 275
            {"@GROUP_NAME_1", "EU"},  // Count: 23, Ordinal: 277
            {"@QUANTITY", "FU"},  // Count: 23, Ordinal: 278
            {"N'@MATERIAL = '", "GU"},  // Count: 23, Ordinal: 279
            {"N'@MATERIAL = NULL'", "HU"},  // Count: 23, Ordinal: 280
            {"PROCESS_LIMITS", "IU"},  // Count: 23, Ordinal: 281
            {"QUALITY_MEASURE_ID", "JU"},  // Count: 23, Ordinal: 282
            {"SMALLINT", "KU"},  // Count: 23, Ordinal: 283
            {"@APPID", "MU"},  // Count: 22, Ordinal: 285
            {"@GROUP_NAME_4", "AV"},  // Count: 22, Ordinal: 286
            {"@NEWNAME", "BV"},  // Count: 22, Ordinal: 287
            {"@UNIT", "CV"},  // Count: 22, Ordinal: 288
            {"@USERID", "DV"},  // Count: 22, Ordinal: 289
            {"CSCITATIONLOG", "EV"},  // Count: 22, Ordinal: 290
            {"G", "FV"},  // Count: 22, Ordinal: 291
            {"LASTALARMTIME", "GV"},  // Count: 22, Ordinal: 292
            {"PI_TAG_NAME", "HV"},  // Count: 22, Ordinal: 293
            {"RESETTIME", "IV"},  // Count: 22, Ordinal: 294
            {"-1", "JV"},  // Count: 21, Ordinal: 295
            {"@MOVEMENT_TYPE", "KV"},  // Count: 21, Ordinal: 296
            {"@PARENTID", "LV"},  // Count: 21, Ordinal: 297
            {"@PHASE", "MV"},  // Count: 21, Ordinal: 298
            {"@ROOT", "AW"},  // Count: 21, Ordinal: 299
            {"@V_SUPPDATA", "BW"},  // Count: 21, Ordinal: 300
            {"B", "CW"},  // Count: 21, Ordinal: 301
            {"CL", "DW"},  // Count: 21, Ordinal: 302
            {"N'%'", "EW"},  // Count: 21, Ordinal: 303
            {"PROCESS_STATE", "FW"},  // Count: 21, Ordinal: 304
            {"STATUS", "GW"},  // Count: 21, Ordinal: 305
            {"TAG", "HW"},  // Count: 21, Ordinal: 306
            {"VARCHAR", "IW"},  // Count: 21, Ordinal: 307
            {"@FULL_PATH", "JW"},  // Count: 20, Ordinal: 308
            {"@PM", "KW"},  // Count: 20, Ordinal: 309
            {"@RESETTIME", "LW"},  // Count: 20, Ordinal: 310
            {"@SAMPLEIDTAGNAME", "MW"},  // Count: 20, Ordinal: 311
            {"N'@BATCH = '", "AX"},  // Count: 20, Ordinal: 312
            {"N'@BATCH = NULL'", "BX"},  // Count: 20, Ordinal: 313
            {"N'@PLANT_ID = '", "CX"},  // Count: 20, Ordinal: 314
            {"N'@PLANT_ID = NULL'", "DX"},  // Count: 20, Ordinal: 315
            {"PIMSROLENAME", "EX"},  // Count: 20, Ordinal: 316
            {"PRODUCT", "FX"},  // Count: 20, Ordinal: 317
            {"RDMTAGS", "GX"},  // Count: 20, Ordinal: 318
            {"2048", "HX"},  // Count: 19, Ordinal: 319
            {"@LEVEL_NUMBER", "IX"},  // Count: 19, Ordinal: 320
            {"@OLDACTIONID", "JX"},  // Count: 19, Ordinal: 321
            {"BATCH", "KX"},  // Count: 19, Ordinal: 322
            {"CAST", "LX"},  // Count: 19, Ordinal: 323
            {"N'@RECIPE_ID = '", "MX"},  // Count: 19, Ordinal: 324
            {"N'@RECIPE_ID = NULL'", "AY"},  // Count: 19, Ordinal: 325
            {"RAISERROR", "BY"},  // Count: 19, Ordinal: 326
            {"@LIST_NAME_1", "EY"},  // Count: 18, Ordinal: 329
            {"@OPERATION", "FY"},  // Count: 18, Ordinal: 330
            {"@ORDER_ITEM_NUMBER", "GY"},  // Count: 18, Ordinal: 331
            {"@PLANT", "HY"},  // Count: 18, Ordinal: 332
            {"@PLANT_OF_RESOURCE", "IY"},  // Count: 18, Ordinal: 333
            {"@RESOURCE", "JY"},  // Count: 18, Ordinal: 334
            {"@STOCK_TYPE", "KY"},  // Count: 18, Ordinal: 335
            {"CSDEADTIMELIST", "LY"},  // Count: 18, Ordinal: 336
            {"GRADE", "MY"},  // Count: 18, Ordinal: 337
            {"LIST_CURSOR", "AZ"},  // Count: 18, Ordinal: 338
            {"N'@EVENT_DATE = '", "BZ"},  // Count: 18, Ordinal: 339
            {"N'@EVENT_DATE = NULL'", "CZ"},  // Count: 18, Ordinal: 340
            {"N'@STORAGE_LOCATION = '", "DZ"},  // Count: 18, Ordinal: 341
            {"N'@STORAGE_LOCATION = NULL'", "EZ"},  // Count: 18, Ordinal: 342
            {"NTUSERORNTGROUPID", "FZ"},  // Count: 18, Ordinal: 343
            {"P", "GZ"},  // Count: 18, Ordinal: 344
            {"RDMTAGNAME", "HZ"},  // Count: 18, Ordinal: 345
            {"RULEID_ALL", "IZ"},  // Count: 18, Ordinal: 346
            {"SUPPDATA_EMAIL", "JZ"},  // Count: 18, Ordinal: 347
            {"SUPPDATA_TAG", "KZ"},  // Count: 18, Ordinal: 348
            {"TBLCSRULEIDS", "LZ"},  // Count: 18, Ordinal: 349
            {"@RDMGRPNAME_2", "MZ"},  // Count: 17, Ordinal: 350
            {"@STRATEGYID_1", "Aa"},  // Count: 17, Ordinal: 351
            {"A", "Ba"},  // Count: 17, Ordinal: 352
            {"CITATIONLOG", "Ca"},  // Count: 17, Ordinal: 353
            {"DEPARTMENT", "Da"},  // Count: 17, Ordinal: 354
            {"GROUP", "Ea"},  // Count: 17, Ordinal: 355
            {"N'@EVENT_TIME = '", "Fa"},  // Count: 17, Ordinal: 356
            {"N'@EVENT_TIME = NULL'", "Ga"},  // Count: 17, Ordinal: 357
            {"N'@PROCESS_ORDER = '", "Ha"},  // Count: 17, Ordinal: 358
            {"N'@PROCESS_ORDER = NULL'", "Ia"},  // Count: 17, Ordinal: 359
            {"N'@UNIT_OF_MEASURE = '", "Ja"},  // Count: 17, Ordinal: 360
            {"N'@UNIT_OF_MEASURE = NULL'", "Ka"},  // Count: 17, Ordinal: 361
            {"SEQUENCE_NUMBER", "La"},  // Count: 17, Ordinal: 362
            {"@BATCH_ID", "Ma"},  // Count: 16, Ordinal: 363
            {"@HIERARCHY", "Ab"},  // Count: 16, Ordinal: 364
            {"@HISTALARMMAILID", "Bb"},  // Count: 16, Ordinal: 365
            {"@HISTLIMITS", "Cb"},  // Count: 16, Ordinal: 366
            {"@HISTORICALALARM", "Db"},  // Count: 16, Ordinal: 367
            {"@LASTCHANGEDATE", "Eb"},  // Count: 16, Ordinal: 368
            {"@LISTAREA_2", "Fb"},  // Count: 16, Ordinal: 369
            {"@PARENTCAUSEID", "Gb"},  // Count: 16, Ordinal: 370
            {"@UOM", "Hb"},  // Count: 16, Ordinal: 371
            {"ACTIONTAGSCURSOR", "Ib"},  // Count: 16, Ordinal: 372
            {"CSCAUSEACTION", "Jb"},  // Count: 16, Ordinal: 373
            {"CSCAUSEACTIONRELATIONS", "Kb"},  // Count: 16, Ordinal: 374
            {"CSMAILBOXNAMES", "Lb"},  // Count: 16, Ordinal: 375
            {"CURRENT", "Mb"},  // Count: 16, Ordinal: 376
            {"DEADTIME", "Ac"},  // Count: 16, Ordinal: 377
            {"DECIMAL", "Bc"},  // Count: 16, Ordinal: 378
            {"PIMSGMN", "Cc"},  // Count: 16, Ordinal: 379
            {"PRODUCTION_UNIT_DESCRIPTION", "Dc"},  // Count: 16, Ordinal: 380
            {"VALUE_DATETIME", "Ec"},  // Count: 16, Ordinal: 381
            {"@CAUSEID_1", "Ic"},  // Count: 15, Ordinal: 385
            {"@CLOSINGLOGID", "Jc"},  // Count: 15, Ordinal: 386
            {"@DEC", "Kc"},  // Count: 15, Ordinal: 387
            {"@MATERIAL_PRODUCED", "Lc"},  // Count: 15, Ordinal: 388
            {"@NAME", "Mc"},  // Count: 15, Ordinal: 389
            {"@POSTING_DATE", "Ad"},  // Count: 15, Ordinal: 390
            {"@SAVEERROR", "Bd"},  // Count: 15, Ordinal: 391
            {"@TO_BATCH", "Cd"},  // Count: 15, Ordinal: 392
            {"@TO_MATERIAL", "Dd"},  // Count: 15, Ordinal: 393
            {"@VALUE", "Ed"},  // Count: 15, Ordinal: 394
            {"'19700101'", "Fd"},  // Count: 15, Ordinal: 395
            {"COMMENTTAG", "Gd"},  // Count: 15, Ordinal: 396
            {"CSACTIONTAGS", "Hd"},  // Count: 15, Ordinal: 397
            {"LIMS_TRIGGER_FLAG", "Id"},  // Count: 15, Ordinal: 398
            {"NTGROUP", "Jd"},  // Count: 15, Ordinal: 399
            {"PIMSCHARTLEVELTYPE", "Kd"},  // Count: 15, Ordinal: 400
            {"PIMSDATAGROUPNAME", "Ld"},  // Count: 15, Ordinal: 401
            {"PROCESS_STATE_TAG_NAME", "Md"},  // Count: 15, Ordinal: 402
            {"PRODUCT_TAG_NAME", "Ae"},  // Count: 15, Ordinal: 403
            {"STR", "Be"},  // Count: 15, Ordinal: 404
            {"STRATEGY", "Ce"},  // Count: 15, Ordinal: 405
            {"SUPPCUR", "De"},  // Count: 15, Ordinal: 406
            {"@BATCH_START", "Ee"},  // Count: 14, Ordinal: 407
            {"@CHILDID", "Fe"},  // Count: 14, Ordinal: 408
            {"@DEPARTMENT", "Ge"},  // Count: 14, Ordinal: 409
            {"@LIMS_TRIGGER_FLAG_6", "He"},  // Count: 14, Ordinal: 410
            {"@LISTAREANAME_1", "Ie"},  // Count: 14, Ordinal: 411
            {"@ORDER_LIST_NAME_1", "Je"},  // Count: 14, Ordinal: 412
            {"@PROCESS_STATE", "Ke"},  // Count: 14, Ordinal: 413
            {"@PROCESS_STATE_TAG_NAME_5", "Le"},  // Count: 14, Ordinal: 414
            {"@PRODUCT_NAME", "Me"},  // Count: 14, Ordinal: 415
            {"@PRODUCT_TAG_NAME_4", "Af"},  // Count: 14, Ordinal: 416
            {"@PRODUCTION_UNIT_DESCRIPTION_3", "Bf"},  // Count: 14, Ordinal: 417
            {"@PRODUCTION_UNIT_NAME_3", "Cf"},  // Count: 14, Ordinal: 418
            {"ALIAS", "Df"},  // Count: 14, Ordinal: 419
            {"APPID", "Ef"},  // Count: 14, Ordinal: 420
            {"CAUSEFULLPATH", "Ff"},  // Count: 14, Ordinal: 421
            {"CSACTIONMASTER", "Gf"},  // Count: 14, Ordinal: 422
            {"DELAYTIME", "Hf"},  // Count: 14, Ordinal: 423
            {"DSPCCALCULATIONSET", "If"},  // Count: 14, Ordinal: 424
            {"INPUTTAG", "Jf"},  // Count: 14, Ordinal: 425
            {"LASTCHANGEDATE", "Kf"},  // Count: 14, Ordinal: 426
            {"LOGICAL", "Lf"},  // Count: 14, Ordinal: 427
            {"LTRIM", "Mf"},  // Count: 14, Ordinal: 428
            {"O", "Ag"},  // Count: 14, Ordinal: 429
            {"PLANT", "Bg"},  // Count: 14, Ordinal: 430
            {"PM", "Cg"},  // Count: 14, Ordinal: 431
            {"PROCESSMESSAGE", "Dg"},  // Count: 14, Ordinal: 432
            {"SL", "Eg"},  // Count: 14, Ordinal: 433
            {"USERTBL", "Fg"},  // Count: 14, Ordinal: 434
            {"XIT:", "Gg"},  // Count: 14, Ordinal: 435
            {"-", "Jg"},  // Count: 13, Ordinal: 438
            {"@GROUP_ID", "Kg"},  // Count: 13, Ordinal: 439
            {"@NEXTSORT", "Lg"},  // Count: 13, Ordinal: 440
            {"@QUALITY_MEASURE_ID", "Mg"},  // Count: 13, Ordinal: 441
            {"@TAG", "Ah"},  // Count: 13, Ordinal: 442
            {"@USERINIT", "Bh"},  // Count: 13, Ordinal: 443
            {"@WHERECLAUSE", "Ch"},  // Count: 13, Ordinal: 444
            {"BATCH_START", "Dh"},  // Count: 13, Ordinal: 445
            {"CSCAUSES", "Eh"},  // Count: 13, Ordinal: 446
            {"CSRULEOPERATORS", "Fh"},  // Count: 13, Ordinal: 447
            {"INALARM", "Gh"},  // Count: 13, Ordinal: 448
            {"LIMITGENCITATION", "Hh"},  // Count: 13, Ordinal: 449
            {"LOG", "Ih"},  // Count: 13, Ordinal: 450
            {"MASK_LEVEL", "Jh"},  // Count: 13, Ordinal: 451
            {"OUTER", "Kh"},  // Count: 13, Ordinal: 452
            {"PIMSDSPCCALCSETNAME", "Lh"},  // Count: 13, Ordinal: 453
            {"WITH", "Mh"},  // Count: 13, Ordinal: 454
            {"@BATCH_END", "Ai"},  // Count: 12, Ordinal: 455
            {"@CAUSEID_2", "Bi"},  // Count: 12, Ordinal: 456
            {"@COMMENT_MANDATORY_FLAG_9", "Ci"},  // Count: 12, Ordinal: 457
            {"@COMMENT_TAG_NAME_7", "Di"},  // Count: 12, Ordinal: 458
            {"@CRITERIA_ID", "Ei"},  // Count: 12, Ordinal: 459
            {"@NOW", "Fi"},  // Count: 12, Ordinal: 460
            {"@NTUSERORNTGROUPID_1", "Gi"},  // Count: 12, Ordinal: 461
            {"@POINT_COUNT", "Hi"},  // Count: 12, Ordinal: 462
            {"@PROCESSMESSAGE", "Ii"},  // Count: 12, Ordinal: 463
            {"@QUANTITY_TYPE", "Ji"},  // Count: 12, Ordinal: 464
            {"@RDMGRPNAME_1", "Ki"},  // Count: 12, Ordinal: 465
            {"@RESULT", "Li"},  // Count: 12, Ordinal: 466
            {"@RETVAL", "Mi"},  // Count: 12, Ordinal: 467
            {"@SL", "Aj"},  // Count: 12, Ordinal: 468
            {"@TAGNAME_2", "Bj"},  // Count: 12, Ordinal: 469
            {"@TO_PLANT", "Cj"},  // Count: 12, Ordinal: 470
            {"@TO_SLOC", "Dj"},  // Count: 12, Ordinal: 471
            {"@USERID_TAG_NAME_8", "Ej"},  // Count: 12, Ordinal: 472
            {"CURRENTVALUE", "Fj"},  // Count: 12, Ordinal: 473
            {"EQUIPMENT", "Gj"},  // Count: 12, Ordinal: 474
            {"GROUP_CURSOR", "Hj"},  // Count: 12, Ordinal: 475
            {"HIERARCHY", "Ij"},  // Count: 12, Ordinal: 476
            {"LC", "Jj"},  // Count: 12, Ordinal: 477
            {"LEVEL_NUMBER", "Kj"},  // Count: 12, Ordinal: 478
            {"LIMIT", "Lj"},  // Count: 12, Ordinal: 479
            {"LOGID", "Mj"},  // Count: 12, Ordinal: 480
            {"NOCOUNT", "Ak"},  // Count: 12, Ordinal: 481
            {"OPERAND", "Bk"},  // Count: 12, Ordinal: 482
            {"ROLES", "Ck"},  // Count: 12, Ordinal: 483
            {"UC", "Dk"},  // Count: 12, Ordinal: 484
            {"UNIT", "Ek"},  // Count: 12, Ordinal: 485
            {"USER_ID", "Fk"},  // Count: 12, Ordinal: 486
            {"XT", "Gk"},  // Count: 12, Ordinal: 487
            {"'//'", "Hk"},  // Count: 11, Ordinal: 488
            {"@@ROWCOUNT", "Ik"},  // Count: 11, Ordinal: 489
            {"@ACTION", "Jk"},  // Count: 11, Ordinal: 490
            {"@LISTNAME_1", "Kk"},  // Count: 11, Ordinal: 491
            {"@MASK_LEVEL", "Lk"},  // Count: 11, Ordinal: 492
            {"@MESSAGE", "Mk"},  // Count: 11, Ordinal: 493
            {"@SERVER_1", "Al"},  // Count: 11, Ordinal: 494
            {"@SPRIORITY", "Bl"},  // Count: 11, Ordinal: 495
            {"@STYPE", "Cl"},  // Count: 11, Ordinal: 496
            {"ACTIONDESCRIPTION", "Dl"},  // Count: 11, Ordinal: 497
            {"SAMPLEID", "El"},  // Count: 11, Ordinal: 498
            {"1000", "Fl"},  // Count: 10, Ordinal: 499
            {"@CHART_ID", "Gl"},  // Count: 10, Ordinal: 500
            {"@CRITERIAEXSTATUS", "Hl"},  // Count: 10, Ordinal: 501
            {"@GROUPNAME_2", "Il"},  // Count: 10, Ordinal: 502
            {"@LEVEL_CONTENT", "Jl"},  // Count: 10, Ordinal: 503
            {"@LOGID", "Kl"},  // Count: 10, Ordinal: 504
            {"@MOVEDB", "Ll"},  // Count: 10, Ordinal: 505
            {"@OPERATORID", "Ml"},  // Count: 10, Ordinal: 506
            {"@PART", "Am"},  // Count: 10, Ordinal: 507
            {"@PROCESS_STATE_1", "Bm"},  // Count: 10, Ordinal: 508
            {"@PRODUCT_NAME_2", "Cm"},  // Count: 10, Ordinal: 509
            {"@RDMTAGNAME_3", "Dm"},  // Count: 10, Ordinal: 510
            {"@RUNDATETIME", "Em"},  // Count: 10, Ordinal: 511
            {"@SEQUENCE", "Fm"},  // Count: 10, Ordinal: 512
            {"@STATE", "Gm"},  // Count: 10, Ordinal: 513
            {"CAUSEDESCRIPTION", "Hm"},  // Count: 10, Ordinal: 514
            {"CHARINDEX", "Im"},  // Count: 10, Ordinal: 515
            {"DBNAMECURSOR", "Jm"},  // Count: 10, Ordinal: 516
            {"IN", "Km"},  // Count: 10, Ordinal: 517
            {"MIN", "Lm"},  // Count: 10, Ordinal: 518
            {"PIMSCONTACTACTION", "Mm"},  // Count: 10, Ordinal: 519
            {"PIMSPITAG", "An"},  // Count: 10, Ordinal: 520
            {"PISERVER", "Bn"},  // Count: 10, Ordinal: 521
            {"PRODUCTION_GROUP", "Cn"},  // Count: 10, Ordinal: 522
            {"QUERY_ID", "Dn"},  // Count: 10, Ordinal: 523
            {"RDMSCANTIME", "En"},  // Count: 10, Ordinal: 524
            {"REPORTQUARTER", "Fn"},  // Count: 10, Ordinal: 525
            {"REPORTYEAR", "Gn"},  // Count: 10, Ordinal: 526
            {"SERVERNAME", "Hn"},  // Count: 10, Ordinal: 527
            {"SPCGRAPHPITAG", "In"},  // Count: 10, Ordinal: 528
            {"SUBLEVEL", "Jn"},  // Count: 10, Ordinal: 529
            {"SUBSTRING", "Kn"},  // Count: 10, Ordinal: 530
            {"UOM", "Ln"},  // Count: 10, Ordinal: 531
            {"''", "Ao"},  // Count: 9, Ordinal: 533
            {"@ALIAS", "Bo"},  // Count: 9, Ordinal: 534
            {"@CHILDCAUSEID", "Co"},  // Count: 9, Ordinal: 535
            {"@COST_CENTER", "Do"},  // Count: 9, Ordinal: 536
            {"@COUNT", "Eo"},  // Count: 9, Ordinal: 537
            {"@CUSTOMER", "Fo"},  // Count: 9, Ordinal: 538
            {"@DELIVERY_COMPLETE", "Go"},  // Count: 9, Ordinal: 539
            {"@DESCRIPTION_2", "Ho"},  // Count: 9, Ordinal: 540
            {"@DESCRIPTION_3", "Io"},  // Count: 9, Ordinal: 541
            {"@END_TIME", "Jo"},  // Count: 9, Ordinal: 542
            {"@ENDEVENTDATE", "Ko"},  // Count: 9, Ordinal: 543
            {"@ENDEVENTTIME", "Lo"},  // Count: 9, Ordinal: 544
            {"@INVENTORY_QUANTITY", "Mo"},  // Count: 9, Ordinal: 545
            {"@ITEMID", "Ap"},  // Count: 9, Ordinal: 546
            {"@LEVEL_TEXT_ALIAS_TAG_MASK", "Bp"},  // Count: 9, Ordinal: 547
            {"@LINK_AVAILABLE", "Cp"},  // Count: 9, Ordinal: 548
            {"@MOVE", "Dp"},  // Count: 9, Ordinal: 549
            {"@PISERVER", "Ep"},  // Count: 9, Ordinal: 550
            {"@PURGEDATE", "Fp"},  // Count: 9, Ordinal: 551
            {"@REASON_FOR_VARIANCE", "Gp"},  // Count: 9, Ordinal: 552
            {"@RESERVATION", "Hp"},  // Count: 9, Ordinal: 553
            {"@RESERVATION_ITEM", "Ip"},  // Count: 9, Ordinal: 554
            {"@SEQ", "Jp"},  // Count: 9, Ordinal: 555
            {"@START_TIME", "Kp"},  // Count: 9, Ordinal: 556
            {"@STARTEVENTDATE", "Lp"},  // Count: 9, Ordinal: 557
            {"@STARTEVENTTIME", "Mp"},  // Count: 9, Ordinal: 558
            {"@STRPATH", "Aq"},  // Count: 9, Ordinal: 559
            {"@TMATERIAL", "Bq"},  // Count: 9, Ordinal: 560
            {"@YIELD_TO_CONFIRM", "Cq"},  // Count: 9, Ordinal: 561
            {"CRITERIAEXSTATUS", "Dq"},  // Count: 9, Ordinal: 562
            {"CSSCHEDULINGINFO", "Eq"},  // Count: 9, Ordinal: 563
            {"DATERESTARTED", "Fq"},  // Count: 9, Ordinal: 564
            {"DOCUMENTURL", "Gq"},  // Count: 9, Ordinal: 565
            {"EXCLUDEPROGRAMCURSOR", "Hq"},  // Count: 9, Ordinal: 566
            {"MODIFIED", "Iq"},  // Count: 9, Ordinal: 567
            {"N'@MSCLA = '", "Jq"},  // Count: 9, Ordinal: 568
            {"N'@MSCLA = NULL'", "Kq"},  // Count: 9, Ordinal: 569
            {"ORDER_LIST_NAME", "Lq"},  // Count: 9, Ordinal: 570
            {"PIDISPLAYURL", "Mq"},  // Count: 9, Ordinal: 571
            {"QMQUALITYMEASUREPOINTS", "Ar"},  // Count: 9, Ordinal: 572
            {"RDMALARMSTATUS", "Br"},  // Count: 9, Ordinal: 573
            {"RULEID_EMAIL", "Cr"},  // Count: 9, Ordinal: 574
            {"RULEID_TAG", "Dr"},  // Count: 9, Ordinal: 575
            {"SAMPLE_TEMPLATE", "Er"},  // Count: 9, Ordinal: 576
            {"SUPPLEMENTALDATAFOREMAIL", "Fr"},  // Count: 9, Ordinal: 577
            {"SUPPLEMENTALDATAFORPRINT", "Gr"},  // Count: 9, Ordinal: 578
            {"SUPPLEMENTALDATAFORTAG", "Hr"},  // Count: 9, Ordinal: 579
            {"@ACTIONTAGNAME", "Jr"},  // Count: 8, Ordinal: 581
            {"@APPLICATION_NAME_1", "Kr"},  // Count: 8, Ordinal: 582
            {"@BETAISAVAILABLE_3", "Lr"},  // Count: 8, Ordinal: 583
            {"@BETARELEASEVERSION_2", "Mr"},  // Count: 8, Ordinal: 584
            {"@CHILDCAUSEID_2", "As"},  // Count: 8, Ordinal: 585
            {"@CRITERIATYPEID", "Bs"},  // Count: 8, Ordinal: 586
            {"@DATE_END", "Cs"},  // Count: 8, Ordinal: 587
            {"@GCCOMPONENT_2", "Ds"},  // Count: 8, Ordinal: 588
            {"@GCMETHOD_1", "Es"},  // Count: 8, Ordinal: 589
            {"@LIST_AREA_NAME_3", "Fs"},  // Count: 8, Ordinal: 590
            {"@LOGICAL", "Gs"},  // Count: 8, Ordinal: 591
            {"@MAXSORT", "Hs"},  // Count: 8, Ordinal: 592
            {"@NEWACTIONID", "Is"},  // Count: 8, Ordinal: 593
            {"@NEWCAUSEID", "Js"},  // Count: 8, Ordinal: 594
            {"@NTGROUP", "Ks"},  // Count: 8, Ordinal: 595
            {"@PARENTCAUSEID_1", "Ls"},  // Count: 8, Ordinal: 596
            {"@PI_NAME_1", "Ms"},  // Count: 8, Ordinal: 597
            {"@PI_TAG_NAME", "At"},  // Count: 8, Ordinal: 598
            {"@PI_TAG_NAME_5", "Bt"},  // Count: 8, Ordinal: 599
            {"@PRIORITY", "Ct"},  // Count: 8, Ordinal: 600
            {"@PROCESSORDER", "Dt"},  // Count: 8, Ordinal: 601
            {"@PRODUCTIONRELEASEVERSION_1", "Et"},  // Count: 8, Ordinal: 602
            {"@QUERY_ID", "Ft"},  // Count: 8, Ordinal: 603
            {"@SAMPLETEMPLATE", "Gt"},  // Count: 8, Ordinal: 604
            {"@SUBLEVEL", "Ht"},  // Count: 8, Ordinal: 605
            {"@TAG_NAME_1", "It"},  // Count: 8, Ordinal: 606
            {"@TAG_NAME_4", "Jt"},  // Count: 8, Ordinal: 607
            {"@TIMESTAMP", "Kt"},  // Count: 8, Ordinal: 608
            {"@VALUE_6", "Lt"},  // Count: 8, Ordinal: 609
            {"@VALUE_DATETIME", "Mt"},  // Count: 8, Ordinal: 610
            {"ACTIONBYUSER", "Au"},  // Count: 8, Ordinal: 611
            {"APPLICATION", "Bu"},  // Count: 8, Ordinal: 612
            {"APPNAME", "Cu"},  // Count: 8, Ordinal: 613
            {"BASE", "Du"},  // Count: 8, Ordinal: 614
            {"BASE_UNIT_NAME", "Eu"},  // Count: 8, Ordinal: 615
            {"BATCH_END", "Fu"},  // Count: 8, Ordinal: 616
            {"CHART_ID", "Gu"},  // Count: 8, Ordinal: 617
            {"COMMENT", "Hu"},  // Count: 8, Ordinal: 618
            {"COMMENT_MANDATORY_FLAG", "Iu"},  // Count: 8, Ordinal: 619
            {"COMMENT_TAG_NAME", "Ju"},  // Count: 8, Ordinal: 620
            {"CSDEADTIMELISTLOG", "Ku"},  // Count: 8, Ordinal: 621
            {"GROUP_ID", "Lu"},  // Count: 8, Ordinal: 622
            {"GRP", "Mu"},  // Count: 8, Ordinal: 623
            {"HISTLIMITSINALARM", "Av"},  // Count: 8, Ordinal: 624
            {"INCLUDEPROGRAMCURSOR", "Bv"},  // Count: 8, Ordinal: 625
            {"LEVEL_TEXT_ALIAS_TAG_MASK", "Cv"},  // Count: 8, Ordinal: 626
            {"LINKED_UNIT_NAME", "Dv"},  // Count: 8, Ordinal: 627
            {"MEASURE_TYPE", "Ev"},  // Count: 8, Ordinal: 628
            {"N'@MATERIAL_CONSUMED = '", "Fv"},  // Count: 8, Ordinal: 629
            {"N'@MATERIAL_CONSUMED = NULL'", "Gv"},  // Count: 8, Ordinal: 630
            {"NAMEDVALUE", "Hv"},  // Count: 8, Ordinal: 631
            {"N'EMAIL'", "Iv"},  // Count: 8, Ordinal: 632
            {"NOOFREMINDERMAILS", "Jv"},  // Count: 8, Ordinal: 633
            {"N'PRINT'", "Kv"},  // Count: 8, Ordinal: 634
            {"N'TAG'", "Lv"},  // Count: 8, Ordinal: 635
            {"PCBO_BATCH", "Mv"},  // Count: 8, Ordinal: 636
            {"PCBO_GMN", "Aw"},  // Count: 8, Ordinal: 637
            {"PCBO_SAMPLE_TRACKING", "Bw"},  // Count: 8, Ordinal: 638
            {"PCM", "Cw"},  // Count: 8, Ordinal: 639
            {"PIMSBATCHID", "Dw"},  // Count: 8, Ordinal: 640
            {"PIMSLEVELNUMBER", "Ew"},  // Count: 8, Ordinal: 641
            {"PIMSNAMEDVALUENAME", "Fw"},  // Count: 8, Ordinal: 642
            {"PIMSQUANTITY", "Gw"},  // Count: 8, Ordinal: 643
            {"PIMSRESOURCE_NETWORK", "Hw"},  // Count: 8, Ordinal: 644
            {"PIMSROLEAPPPRIVS", "Iw"},  // Count: 8, Ordinal: 645
            {"PIUSERID", "Jw"},  // Count: 8, Ordinal: 646
            {"POINT_COUNT", "Kw"},  // Count: 8, Ordinal: 647
            {"PREC", "Lw"},  // Count: 8, Ordinal: 648
            {"PUBLIC_FLAG", "Mw"},  // Count: 8, Ordinal: 649
            {"QMQUALITYMEASUREDATA", "Ax"},  // Count: 8, Ordinal: 650
            {"RDMACTIVESTATUS", "Bx"},  // Count: 8, Ordinal: 651
            {"RDMALARMOFFSET", "Cx"},  // Count: 8, Ordinal: 652
            {"RDMGRPDESC", "Dx"},  // Count: 8, Ordinal: 653
            {"RDMINTERVAL", "Ex"},  // Count: 8, Ordinal: 654
            {"RDMINTERVALUNIT", "Fx"},  // Count: 8, Ordinal: 655
            {"RDMSTARTMONTH", "Gx"},  // Count: 8, Ordinal: 656
            {"RDMSTARTTIME", "Hx"},  // Count: 8, Ordinal: 657
            {"RDMSTARTWEEKDAY", "Ix"},  // Count: 8, Ordinal: 658
            {"RUNDATETIME", "Jx"},  // Count: 8, Ordinal: 659
            {"STRATEGYTAGSCURSOR", "Kx"},  // Count: 8, Ordinal: 660
            {"TARGET_FLAG", "Lx"},  // Count: 8, Ordinal: 661
            {"TARGET_TAG_NAME", "Mx"},  // Count: 8, Ordinal: 662
            {"TEXTDATA", "Ay"},  // Count: 8, Ordinal: 663
            {"THEN", "By"},  // Count: 8, Ordinal: 664
            {"USERID_TAG_NAME", "Cy"},  // Count: 8, Ordinal: 665
            {"USERROLES", "Dy"},  // Count: 8, Ordinal: 666
            {"WHEN", "Ey"},  // Count: 8, Ordinal: 667
            {"@BUCKET", "Gy"},  // Count: 7, Ordinal: 669
            {"@DELAYTIME", "Hy"},  // Count: 7, Ordinal: 670
            {"@DELV02", "Iy"},  // Count: 7, Ordinal: 671
            {"@LIST_NAME_3", "Jy"},  // Count: 7, Ordinal: 672
            {"@LISTNAME", "Ky"},  // Count: 7, Ordinal: 673
            {"@NEWTAG", "Ly"},  // Count: 7, Ordinal: 674
            {"@NO", "My"},  // Count: 7, Ordinal: 675
            {"@OLDTAG", "Az"},  // Count: 7, Ordinal: 676
            {"@OUT_LINK_EXIST_FLAG", "Bz"},  // Count: 7, Ordinal: 677
            {"@PRODUCT_GMN", "Cz"},  // Count: 7, Ordinal: 678
            {"@SERVER_NAME_1", "Dz"},  // Count: 7, Ordinal: 679
            {"@SERVERNAME", "Ez"},  // Count: 7, Ordinal: 680
            {"@TRACEPATH", "Fz"},  // Count: 7, Ordinal: 681
            {"@V_RULEID", "Gz"},  // Count: 7, Ordinal: 682
            {"'\\'", "Hz"},  // Count: 7, Ordinal: 683      - remember that '\' is '' in c#, must '\\' to get '\' - took hours to find this one
            {"<>0", "Iz"},  // Count: 7, Ordinal: 684
            {"APPLICATIONNAME", "Jz"},  // Count: 7, Ordinal: 685
            {"BREAK", "Kz"},  // Count: 7, Ordinal: 686
            {"CASE", "Lz"},  // Count: 7, Ordinal: 687
            {"CSCRITERIATYPES", "Mz"},  // Count: 7, Ordinal: 688
            {"CSMAILBOXES", "AAA"},  // Count: 7, Ordinal: 689
            {"CSSTRATEGYTAGS", "BAA"},  // Count: 7, Ordinal: 690
            {"DSPCCALCULATIONSETTAGS", "CAA"},  // Count: 7, Ordinal: 691
            {"GROUP_DESCRIPTION", "DAA"},  // Count: 7, Ordinal: 692
            {"HIST", "EAA"},  // Count: 7, Ordinal: 693
            {"LINK", "FAA"},  // Count: 7, Ordinal: 694
            {"LIST_DESCRIPTION", "GAA"},  // Count: 7, Ordinal: 695
            {"M", "HAA"},  // Count: 7, Ordinal: 696
            {"N'@MOVEMENT_TYPE = '", "IAA"},  // Count: 7, Ordinal: 697
            {"N'@MOVEMENT_TYPE = NULL'", "JAA"},  // Count: 7, Ordinal: 698
            {"N'@PHASE = '", "KAA"},  // Count: 7, Ordinal: 699
            {"N'@PHASE = NULL'", "LAA"},  // Count: 7, Ordinal: 700
            {"NAME", "MAA"},  // Count: 7, Ordinal: 701
            {"PARAMETER", "ABA"},  // Count: 7, Ordinal: 702
            {"PARAMETERS", "BBA"},  // Count: 7, Ordinal: 703
            {"PIMSROLEGROUPPRIVS", "CBA"},  // Count: 7, Ordinal: 704
            {"PIMSROLES", "DBA"},  // Count: 7, Ordinal: 705
            {"PIMSSHIPPEDBATCHES", "EBA"},  // Count: 7, Ordinal: 706
            {"QUANTITY", "FBA"},  // Count: 7, Ordinal: 707
            {"RDMALTAG", "GBA"},  // Count: 7, Ordinal: 708
            {"RDMSTATUS", "HBA"},  // Count: 7, Ordinal: 709
            {"REFACTIONID", "IBA"},  // Count: 7, Ordinal: 710
            {"REFDATECREATED", "JBA"},  // Count: 7, Ordinal: 711
            {"REFRESETTIME", "KBA"},  // Count: 7, Ordinal: 712
            {"S", "LBA"},  // Count: 7, Ordinal: 713
            {"SCROLL", "MBA"},  // Count: 7, Ordinal: 714
            {"SECPIMSADMINSUBLEVEL", "ACA"},  // Count: 7, Ordinal: 715
            {"STAGE", "BCA"},  // Count: 7, Ordinal: 716
            {"STATUSID", "CCA"},  // Count: 7, Ordinal: 717
            {"SUPDATA", "DCA"},  // Count: 7, Ordinal: 718
            {"TARGET_VALUE", "ECA"},  // Count: 7, Ordinal: 719
            {"TEST", "FCA"},  // Count: 7, Ordinal: 720
            {"USERID", "GCA"},  // Count: 7, Ordinal: 721
            {"@ACCESS", "JCA"},  // Count: 6, Ordinal: 724
            {"@ACTIONID_2", "KCA"},  // Count: 6, Ordinal: 725
            {"@ACTIVITY4", "LCA"},  // Count: 6, Ordinal: 726
            {"@ACTIVITY4_UNITS", "MCA"},  // Count: 6, Ordinal: 727
            {"@ACTIVITY6", "ADA"},  // Count: 6, Ordinal: 728
            {"@ACTIVITY6_UNITS", "BDA"},  // Count: 6, Ordinal: 729
            {"@ADDBATCHES", "CDA"},  // Count: 6, Ordinal: 730
            {"@ADDBD01", "DDA"},  // Count: 6, Ordinal: 731
            {"@ADDBD02", "EDA"},  // Count: 6, Ordinal: 732
            {"@ADDBD03", "FDA"},  // Count: 6, Ordinal: 733
            {"@ADDBD04", "GDA"},  // Count: 6, Ordinal: 734
            {"@ADDBD05", "HDA"},  // Count: 6, Ordinal: 735
            {"@ADDBD06", "IDA"},  // Count: 6, Ordinal: 736
            {"@ADDBD07", "JDA"},  // Count: 6, Ordinal: 737
            {"@ADDBD08", "KDA"},  // Count: 6, Ordinal: 738
            {"@ADDBD09", "LDA"},  // Count: 6, Ordinal: 739
            {"@ADDBD10", "MDA"},  // Count: 6, Ordinal: 740
            {"@ADDBD11", "AEA"},  // Count: 6, Ordinal: 741
            {"@ADDBD12", "BEA"},  // Count: 6, Ordinal: 742
            {"@ADDBD13", "CEA"},  // Count: 6, Ordinal: 743
            {"@ADDBD14", "DEA"},  // Count: 6, Ordinal: 744
            {"@ADDBD15", "EEA"},  // Count: 6, Ordinal: 745
            {"@ADDBD16", "FEA"},  // Count: 6, Ordinal: 746
            {"@ADDBD17", "GEA"},  // Count: 6, Ordinal: 747
            {"@ADDBD18", "HEA"},  // Count: 6, Ordinal: 748
            {"@ADDBD19", "IEA"},  // Count: 6, Ordinal: 749
            {"@ADDBD20", "JEA"},  // Count: 6, Ordinal: 750
            {"@ADDBD21", "KEA"},  // Count: 6, Ordinal: 751
            {"@ADDBD22", "LEA"},  // Count: 6, Ordinal: 752
            {"@ADDBD23", "MEA"},  // Count: 6, Ordinal: 753
            {"@ADDBD24", "AFA"},  // Count: 6, Ordinal: 754
            {"@ADDBD25", "BFA"},  // Count: 6, Ordinal: 755
            {"@ADDBD26", "CFA"},  // Count: 6, Ordinal: 756
            {"@ADDBD27", "DFA"},  // Count: 6, Ordinal: 757
            {"@ADDBD28", "EFA"},  // Count: 6, Ordinal: 758
            {"@ADDBD29", "FFA"},  // Count: 6, Ordinal: 759
            {"@ADDBD30", "GFA"},  // Count: 6, Ordinal: 760
            {"@AMOUNT_IN_LOCAL_CURRENCY", "HFA"},  // Count: 6, Ordinal: 761
            {"@ANALYSIS", "IFA"},  // Count: 6, Ordinal: 762
            {"@APPLICATIONNAME_1", "JFA"},  // Count: 6, Ordinal: 763
            {"@APPNAME_2", "KFA"},  // Count: 6, Ordinal: 764
            {"@BATCH_LINK", "LFA"},  // Count: 6, Ordinal: 765
            {"@BATCH_PROD", "MFA"},  // Count: 6, Ordinal: 766
            {"@BATCH_TYPE", "AGA"},  // Count: 6, Ordinal: 767
            {"@BATCH_WHERE", "BGA"},  // Count: 6, Ordinal: 768
            {"@BEFORETIME", "CGA"},  // Count: 6, Ordinal: 769
            {"@CITATIONRESPONSEID", "DGA"},  // Count: 6, Ordinal: 770
            {"@COMPONENT", "EGA"},  // Count: 6, Ordinal: 771
            {"@CONTROL_RECIPE", "FGA"},  // Count: 6, Ordinal: 772
            {"@CONTROL_RECIPE_STATUS", "GGA"},  // Count: 6, Ordinal: 773
            {"@COPYRESETTAGS", "HGA"},  // Count: 6, Ordinal: 774
            {"@DATERESTARTED", "IGA"},  // Count: 6, Ordinal: 775
            {"@DATETIME_3", "JGA"},  // Count: 6, Ordinal: 776
            {"@DELBATCHES", "KGA"},  // Count: 6, Ordinal: 777
            {"@DELBD01", "LGA"},  // Count: 6, Ordinal: 778
            {"@DELBD02", "MGA"},  // Count: 6, Ordinal: 779
            {"@DELBD03", "AHA"},  // Count: 6, Ordinal: 780
            {"@DELBD04", "BHA"},  // Count: 6, Ordinal: 781
            {"@DELBD05", "CHA"},  // Count: 6, Ordinal: 782
            {"@DELBD06", "DHA"},  // Count: 6, Ordinal: 783
            {"@DELBD07", "EHA"},  // Count: 6, Ordinal: 784
            {"@DELBD08", "FHA"},  // Count: 6, Ordinal: 785
            {"@DELBD09", "GHA"},  // Count: 6, Ordinal: 786
            {"@DELBD10", "HHA"},  // Count: 6, Ordinal: 787
            {"@DELBD11", "IHA"},  // Count: 6, Ordinal: 788
            {"@DELBD12", "JHA"},  // Count: 6, Ordinal: 789
            {"@DELBD13", "KHA"},  // Count: 6, Ordinal: 790
            {"@DELBD14", "LHA"},  // Count: 6, Ordinal: 791
            {"@DELBD15", "MHA"},  // Count: 6, Ordinal: 792
            {"@DELBD16", "AIA"},  // Count: 6, Ordinal: 793
            {"@DELBD17", "BIA"},  // Count: 6, Ordinal: 794
            {"@DELBD18", "CIA"},  // Count: 6, Ordinal: 795
            {"@DELBD19", "DIA"},  // Count: 6, Ordinal: 796
            {"@DELBD20", "EIA"},  // Count: 6, Ordinal: 797
            {"@DELBD21", "FIA"},  // Count: 6, Ordinal: 798
            {"@DELBD22", "GIA"},  // Count: 6, Ordinal: 799
            {"@DELBD23", "HIA"},  // Count: 6, Ordinal: 800
            {"@DELBD24", "IIA"},  // Count: 6, Ordinal: 801
            {"@DELBD25", "JIA"},  // Count: 6, Ordinal: 802
            {"@DELBD26", "KIA"},  // Count: 6, Ordinal: 803
            {"@DELBD27", "LIA"},  // Count: 6, Ordinal: 804
            {"@DELBD28", "MIA"},  // Count: 6, Ordinal: 805
            {"@DELBD29", "AJA"},  // Count: 6, Ordinal: 806
            {"@DELBD30", "BJA"},  // Count: 6, Ordinal: 807
            {"@DELIVERY_NOTE", "CJA"},  // Count: 6, Ordinal: 808
            {"@DELV01", "DJA"},  // Count: 6, Ordinal: 809
            {"@DELV03", "EJA"},  // Count: 6, Ordinal: 810
            {"@DELV04", "FJA"},  // Count: 6, Ordinal: 811
            {"@DELV05", "GJA"},  // Count: 6, Ordinal: 812
            {"@DELV06", "HJA"},  // Count: 6, Ordinal: 813
            {"@DELV07", "IJA"},  // Count: 6, Ordinal: 814
            {"@DELV08", "JJA"},  // Count: 6, Ordinal: 815
            {"@DELV09", "KJA"},  // Count: 6, Ordinal: 816
            {"@DELV10", "LJA"},  // Count: 6, Ordinal: 817
            {"@DELV11", "MJA"},  // Count: 6, Ordinal: 818
            {"@DELV12", "AKA"},  // Count: 6, Ordinal: 819
            {"@DELV13", "BKA"},  // Count: 6, Ordinal: 820
            {"@DELV14", "CKA"},  // Count: 6, Ordinal: 821
            {"@DELV15", "DKA"},  // Count: 6, Ordinal: 822
            {"@DELV16", "EKA"},  // Count: 6, Ordinal: 823
            {"@DELV17", "FKA"},  // Count: 6, Ordinal: 824
            {"@DELV18", "GKA"},  // Count: 6, Ordinal: 825
            {"@DELV19", "HKA"},  // Count: 6, Ordinal: 826
            {"@DELV20", "IKA"},  // Count: 6, Ordinal: 827
            {"@DELV21", "JKA"},  // Count: 6, Ordinal: 828
            {"@DELV22", "KKA"},  // Count: 6, Ordinal: 829
            {"@DELV23", "LKA"},  // Count: 6, Ordinal: 830
            {"@DELV24", "MKA"},  // Count: 6, Ordinal: 831
            {"@DELV25", "ALA"},  // Count: 6, Ordinal: 832
            {"@DELV26", "BLA"},  // Count: 6, Ordinal: 833
            {"@DELV27", "CLA"},  // Count: 6, Ordinal: 834
            {"@DELV28", "DLA"},  // Count: 6, Ordinal: 835
            {"@DELV29", "ELA"},  // Count: 6, Ordinal: 836
            {"@DELV30", "FLA"},  // Count: 6, Ordinal: 837
            {"@EQUIPMENT_NAME_1", "GLA"},  // Count: 6, Ordinal: 838
            {"@EVERY_N_POINTS", "HLA"},  // Count: 6, Ordinal: 839
            {"@EXCLUDE_RESAMPLES", "ILA"},  // Count: 6, Ordinal: 840
            {"@EXCLUDE_REWORKS", "JLA"},  // Count: 6, Ordinal: 841
            {"@EXCLUDE_SYNTHETICS", "KLA"},  // Count: 6, Ordinal: 842
            {"@FILTER1_LINK", "LLA"},  // Count: 6, Ordinal: 843
            {"@FILTER1_NAME", "MLA"},  // Count: 6, Ordinal: 844
            {"@FILTER1_TYPE", "AMA"},  // Count: 6, Ordinal: 845
            {"@FILTER2_LINK", "BMA"},  // Count: 6, Ordinal: 846
            {"@FILTER2_NAME", "CMA"},  // Count: 6, Ordinal: 847
            {"@FILTER2_TYPE", "DMA"},  // Count: 6, Ordinal: 848
            {"@FINISH_END_TIME", "EMA"},  // Count: 6, Ordinal: 849
            {"@FINISH_START_TIME", "FMA"},  // Count: 6, Ordinal: 850
            {"@FORMULA", "GMA"},  // Count: 6, Ordinal: 851
            {"@GIFLAG", "HMA"},  // Count: 6, Ordinal: 852
            {"@GMN_WHERE", "IMA"},  // Count: 6, Ordinal: 853
            {"@GROUP_FILTER", "JMA"},  // Count: 6, Ordinal: 854
            {"@GROUP_NAME_5", "KMA"},  // Count: 6, Ordinal: 855
            {"@GROUP_NAME_6", "LMA"},  // Count: 6, Ordinal: 856
            {"@GROUP_NAME_7", "MMA"},  // Count: 6, Ordinal: 857
            {"@GROUPID_1", "ANA"},  // Count: 6, Ordinal: 858
            {"@HOWMANY", "BNA"},  // Count: 6, Ordinal: 859
            {"@INSTRUCTIONS", "CNA"},  // Count: 6, Ordinal: 860
            {"@JOB_NAME", "DNA"},  // Count: 6, Ordinal: 861
            {"@LIMS_INSTANCE", "ENA"},  // Count: 6, Ordinal: 862
            {"@LIMS_SERVER", "FNA"},  // Count: 6, Ordinal: 863
            {"@LIST_AREA_NAME_1", "GNA"},  // Count: 6, Ordinal: 864
            {"@LIST_AREA_NAME_5", "HNA"},  // Count: 6, Ordinal: 865
            {"@LIST_NAME_2", "INA"},  // Count: 6, Ordinal: 866
            {"@LISTPOINT_0", "JNA"},  // Count: 6, Ordinal: 867
            {"@LOGIN_END_TIME", "KNA"},  // Count: 6, Ordinal: 868
            {"@LOGIN_LOCATION", "LNA"},  // Count: 6, Ordinal: 869
            {"@LOGIN_START_TIME", "MNA"},  // Count: 6, Ordinal: 870
            {"@MEANSTRANSPORT", "AOA"},  // Count: 6, Ordinal: 871
            {"@NAME1", "BOA"},  // Count: 6, Ordinal: 872
            {"@NAME10", "COA"},  // Count: 6, Ordinal: 873
            {"@NAME11", "DOA"},  // Count: 6, Ordinal: 874
            {"@NAME12", "EOA"},  // Count: 6, Ordinal: 875
            {"@NAME13", "FOA"},  // Count: 6, Ordinal: 876
            {"@NAME14", "GOA"},  // Count: 6, Ordinal: 877
            {"@NAME15", "HOA"},  // Count: 6, Ordinal: 878
            {"@NAME16", "IOA"},  // Count: 6, Ordinal: 879
            {"@NAME17", "JOA"},  // Count: 6, Ordinal: 880
            {"@NAME18", "KOA"},  // Count: 6, Ordinal: 881
            {"@NAME19", "LOA"},  // Count: 6, Ordinal: 882
            {"@NAME2", "MOA"},  // Count: 6, Ordinal: 883
            {"@NAME20", "APA"},  // Count: 6, Ordinal: 884
            {"@NAME3", "BPA"},  // Count: 6, Ordinal: 885
            {"@NAME4", "CPA"},  // Count: 6, Ordinal: 886
            {"@NAME5", "DPA"},  // Count: 6, Ordinal: 887
            {"@NAME6", "EPA"},  // Count: 6, Ordinal: 888
            {"@NAME7", "FPA"},  // Count: 6, Ordinal: 889
            {"@NAME8", "GPA"},  // Count: 6, Ordinal: 890
            {"@NAME9", "HPA"},  // Count: 6, Ordinal: 891
            {"@NAMEDVALUE", "IPA"},  // Count: 6, Ordinal: 892
            {"@NEWCHILDCAUSE", "JPA"},  // Count: 6, Ordinal: 893
            {"@OLDDES", "KPA"},  // Count: 6, Ordinal: 894
            {"@OLDLIMS", "LPA"},  // Count: 6, Ordinal: 895
            {"@OLDPROCTAG", "MPA"},  // Count: 6, Ordinal: 896
            {"@OLDPRODTAG", "AQA"},  // Count: 6, Ordinal: 897
            {"@OUT_INSERT", "BQA"},  // Count: 6, Ordinal: 898
            {"@PARTNUMBER", "CQA"},  // Count: 6, Ordinal: 899
            {"@PCNAME", "DQA"},  // Count: 6, Ordinal: 900
            {"@PI_SERVER", "EQA"},  // Count: 6, Ordinal: 901
            {"@PISERVERNAME", "FQA"},  // Count: 6, Ordinal: 902
            {"@PLANT_WHERE", "GQA"},  // Count: 6, Ordinal: 903
            {"@PM_NUMBER", "HQA"},  // Count: 6, Ordinal: 904
            {"@POSTINGDATE", "IQA"},  // Count: 6, Ordinal: 905
            {"@PROCESSMESSAGE_WHERE", "JQA"},  // Count: 6, Ordinal: 906
            {"@PRODUCT_NAME_1", "KQA"},  // Count: 6, Ordinal: 907
            {"@PRODUCTION_UNIT_NAME_4", "LQA"},  // Count: 6, Ordinal: 908
            {"@PUBLIC_FLAG", "MQA"},  // Count: 6, Ordinal: 909
            {"@QMID", "ARA"},  // Count: 6, Ordinal: 910
            {"@QUALITY_STATUS", "BRA"},  // Count: 6, Ordinal: 911
            {"@QUANTITYTYPE", "CRA"},  // Count: 6, Ordinal: 912
            {"@RAWGMN", "DRA"},  // Count: 6, Ordinal: 913
            {"@REJ_MATERIAL", "ERA"},  // Count: 6, Ordinal: 914
            {"@REJ_PROCESS_ORDER", "FRA"},  // Count: 6, Ordinal: 915
            {"@REJ_QUANTITY", "GRA"},  // Count: 6, Ordinal: 916
            {"@RELATIVE_END", "HRA"},  // Count: 6, Ordinal: 917
            {"@RELATIVE_FINISH_FLAG", "IRA"},  // Count: 6, Ordinal: 918
            {"@RELATIVE_LOGIN_FLAG", "JRA"},  // Count: 6, Ordinal: 919
            {"@RELATIVE_START", "KRA"},  // Count: 6, Ordinal: 920
            {"@RELATIVE_TIME_FLAG", "LRA"},  // Count: 6, Ordinal: 921
            {"@REPLACE_STRING_BATCH", "MRA"},  // Count: 6, Ordinal: 922
            {"@REPLACE_STRING_FILTER1", "ASA"},  // Count: 6, Ordinal: 923
            {"@REPLACE_STRING_FILTER2", "BSA"},  // Count: 6, Ordinal: 924
            {"@REPORT_QUARTER_1", "CSA"},  // Count: 6, Ordinal: 925
            {"@REPORT_YEAR_2", "DSA"},  // Count: 6, Ordinal: 926
            {"@RETURNVALUE", "ESA"},  // Count: 6, Ordinal: 927
            {"@ROLENAME_3", "FSA"},  // Count: 6, Ordinal: 928
            {"@SAMPLE_CODE", "GSA"},  // Count: 6, Ordinal: 929
            {"@SAMPLE_POINT", "HSA"},  // Count: 6, Ordinal: 930
            {"@SAMPLE_TEMPLATE", "ISA"},  // Count: 6, Ordinal: 931
            {"@SAMPLE_TYPE", "JSA"},  // Count: 6, Ordinal: 932
            {"@SEALS", "KSA"},  // Count: 6, Ordinal: 933
            {"@SEQUENCE_NUMBER_9", "LSA"},  // Count: 6, Ordinal: 934
            {"@SHIP_TO", "MSA"},  // Count: 6, Ordinal: 935
            {"@SHIPPOINT", "ATA"},  // Count: 6, Ordinal: 936
            {"@SL_WHERE", "BTA"},  // Count: 6, Ordinal: 937
            {"@SQCDERIVESTATUS", "CTA"},  // Count: 6, Ordinal: 938
            {"@STAGE", "DTA"},  // Count: 6, Ordinal: 939
            {"@STATUS_CONF", "ETA"},  // Count: 6, Ordinal: 940
            {"@TAGNAME_4", "FTA"},  // Count: 6, Ordinal: 941
            {"@TARGET_VALUE", "GTA"},  // Count: 6, Ordinal: 942
            {"@TEST_SCHEDULE", "HTA"},  // Count: 6, Ordinal: 943
            {"@TOOLTIP", "ITA"},  // Count: 6, Ordinal: 944
            {"@TRANSPORTID", "JTA"},  // Count: 6, Ordinal: 945
            {"@UNIT_FILTER", "KTA"},  // Count: 6, Ordinal: 946
            {"@USER_ID", "LTA"},  // Count: 6, Ordinal: 947
            {"@V_SUPPACTIONID", "MTA"},  // Count: 6, Ordinal: 948
            {"@VALUE1", "AUA"},  // Count: 6, Ordinal: 949
            {"@VALUE11", "BUA"},  // Count: 6, Ordinal: 950
            {"@VALUE12", "CUA"},  // Count: 6, Ordinal: 951
            {"@VALUE13", "DUA"},  // Count: 6, Ordinal: 952
            {"@VALUE14", "EUA"},  // Count: 6, Ordinal: 953
            {"@VALUE15", "FUA"},  // Count: 6, Ordinal: 954
            {"@VALUE16", "GUA"},  // Count: 6, Ordinal: 955
            {"@VALUE17", "HUA"},  // Count: 6, Ordinal: 956
            {"@VALUE18", "IUA"},  // Count: 6, Ordinal: 957
            {"@VALUE19", "JUA"},  // Count: 6, Ordinal: 958
            {"@VALUE2", "KUA"},  // Count: 6, Ordinal: 959
            {"@VALUE3", "LUA"},  // Count: 6, Ordinal: 960
            {"@VALUE4", "MUA"},  // Count: 6, Ordinal: 961
            {"@VALUE5", "AVA"},  // Count: 6, Ordinal: 962
            {"@VALUE6", "BVA"},  // Count: 6, Ordinal: 963
            {"@VALUE7", "CVA"},  // Count: 6, Ordinal: 964
            {"@VALUE8", "DVA"},  // Count: 6, Ordinal: 965
            {"@VALUE9", "EVA"},  // Count: 6, Ordinal: 966
            {"@VCOUNT2", "FVA"},  // Count: 6, Ordinal: 967
            {"ASC", "GVA"},  // Count: 6, Ordinal: 968
            {"BETAISAVAILABLE", "HVA"},  // Count: 6, Ordinal: 969
            {"BETARELEASEVERSION", "IVA"},  // Count: 6, Ordinal: 970
            {"BIGINT", "JVA"},  // Count: 6, Ordinal: 971
            {"CT", "KVA"},  // Count: 6, Ordinal: 972
            {"DATEADD", "LVA"},  // Count: 6, Ordinal: 973
            {"DATETIMEOFLASTCALCULATION", "MVA"},  // Count: 6, Ordinal: 974
            {"DESC'", "AWA"},  // Count: 6, Ordinal: 975
            {"DISABLE", "BWA"},  // Count: 6, Ordinal: 976
            {"GCCOMPONENT", "CWA"},  // Count: 6, Ordinal: 977
            {"GCMETHOD", "DWA"},  // Count: 6, Ordinal: 978
            {"GROUP_PROMPT", "EWA"},  // Count: 6, Ordinal: 979
            {"INTERP", "FWA"},  // Count: 6, Ordinal: 980
            {"ISNTGROUP", "GWA"},  // Count: 6, Ordinal: 981
            {"LEVEL_CONTENT", "HWA"},  // Count: 6, Ordinal: 982
            {"LINKED_TAG_NAME", "IWA"},  // Count: 6, Ordinal: 983
            {"LIST_AREA_DESCRIPTION", "JWA"},  // Count: 6, Ordinal: 984
            {"LIST_AREA_PROMPT", "KWA"},  // Count: 6, Ordinal: 985
            {"LISTA_CURSOR", "LWA"},  // Count: 6, Ordinal: 986
            {"MASK", "MWA"},  // Count: 6, Ordinal: 987
            {"MAX", "AXA"},  // Count: 6, Ordinal: 988
            {"MODIFICATIONTIMESTAMP", "BXA"},  // Count: 6, Ordinal: 989
            {"N'@OPERATION = '", "CXA"},  // Count: 6, Ordinal: 990
            {"N'@OPERATION = NULL'", "DXA"},  // Count: 6, Ordinal: 991
            {"N'@ORDER_ITEM_NUMBER = '", "EXA"},  // Count: 6, Ordinal: 992
            {"N'@ORDER_ITEM_NUMBER = NULL'", "FXA"},  // Count: 6, Ordinal: 993
            {"N'@PLANT_OF_RESOURCE = '", "GXA"},  // Count: 6, Ordinal: 994
            {"N'@PLANT_OF_RESOURCE = NULL'", "HXA"},  // Count: 6, Ordinal: 995
            {"N'NO RESPONSE'", "IXA"},  // Count: 6, Ordinal: 996
            {"PCLINKS", "JXA"},  // Count: 6, Ordinal: 997
            {"PIMS", "KXA"},  // Count: 6, Ordinal: 998
            {"PIMSUSERID", "LXA"},  // Count: 6, Ordinal: 999
            {"PIMSUSERS", "MXA"},  // Count: 6, Ordinal: 1000
            {"PITAGNAME", "AYA"},  // Count: 6, Ordinal: 1001
            {"POINT_STATE", "BYA"},  // Count: 6, Ordinal: 1002
            {"PRODUCTIONRELEASEVERSION", "CYA"},  // Count: 6, Ordinal: 1003
            {"PROMPTFORPIPASSWORD", "DYA"},  // Count: 6, Ordinal: 1004
            {"RDMACTIVE", "EYA"},  // Count: 6, Ordinal: 1005
            {"ROLEDESCRIPTION", "FYA"},  // Count: 6, Ordinal: 1006
            {"SCREEN_FLAG", "GYA"},  // Count: 6, Ordinal: 1007
            {"SENTSUCCESSFUL", "HYA"},  // Count: 6, Ordinal: 1008
            {"STRAT", "IYA"},  // Count: 6, Ordinal: 1009
            {"TAGS_CURSOR", "JYA"},  // Count: 6, Ordinal: 1010
            {"YINVADJ_PROCMSGDATA", "KYA"},  // Count: 6, Ordinal: 1011
            {"3000", "MYA"},  // Count: 5, Ordinal: 1013
            {"'''", "AZA"},  // Count: 5, Ordinal: 1014
            {"@@INSERTKEY", "BZA"},  // Count: 5, Ordinal: 1015
            {"@BITMAP", "CZA"},  // Count: 5, Ordinal: 1016
            {"@C2", "DZA"},  // Count: 5, Ordinal: 1017
            {"@CGTYPE", "EZA"},  // Count: 5, Ordinal: 1018
            {"@CITATIONID_2", "FZA"},  // Count: 5, Ordinal: 1019
            {"@CURRDATE", "GZA"},  // Count: 5, Ordinal: 1020
            {"@DATABASEID", "HZA"},  // Count: 5, Ordinal: 1021
            {"@DOCUMENTURL", "IZA"},  // Count: 5, Ordinal: 1022
            {"@ERRORNUMBER", "JZA"},  // Count: 5, Ordinal: 1023
            {"@EXCLUDEPROGRAM", "KZA"},  // Count: 5, Ordinal: 1024
            {"@FILENAME", "LZA"},  // Count: 5, Ordinal: 1025
            {"@FULL_PATH_SEGMENT", "MZA"},  // Count: 5, Ordinal: 1026
            {"@HNDL1", "AaA"},  // Count: 5, Ordinal: 1027
            {"@INCLUDEPROGRAM", "BaA"},  // Count: 5, Ordinal: 1028
            {"@INTOPTION", "CaA"},  // Count: 5, Ordinal: 1029
            {"@LINK", "DaA"},  // Count: 5, Ordinal: 1030
            {"@LIST_1", "EaA"},  // Count: 5, Ordinal: 1031
            {"@NUMBER", "FaA"},  // Count: 5, Ordinal: 1032
            {"@PIDISPLAYURL", "GaA"},  // Count: 5, Ordinal: 1033
            {"@PITAGNAME", "HaA"},  // Count: 5, Ordinal: 1034
            {"@PKC1", "IaA"},  // Count: 5, Ordinal: 1035
            {"@PREDB", "JaA"},  // Count: 5, Ordinal: 1036
            {"@REPLACE_STRING", "KaA"},  // Count: 5, Ordinal: 1037
            {"@RESPONSE", "LaA"},  // Count: 5, Ordinal: 1038
            {"@SCREEN_FLAG", "MaA"},  // Count: 5, Ordinal: 1039
            {"@SPCGRAPHPITAG", "AbA"},  // Count: 5, Ordinal: 1040
            {"@STATUSID", "BbA"},  // Count: 5, Ordinal: 1041
            {"APPLICATION_NAME", "CbA"},  // Count: 5, Ordinal: 1042
            {"ARCHIVE", "DbA"},  // Count: 5, Ordinal: 1043
            {"ASC'", "EbA"},  // Count: 5, Ordinal: 1044
            {"BASE_TAG_NAME", "FbA"},  // Count: 5, Ordinal: 1045
            {"CALCULATIONRECURRENCEPATTERNDAYOFMONTH", "GbA"},  // Count: 5, Ordinal: 1046
            {"CALCULATIONRECURRENCEPATTERNDAYOFWEEK", "HbA"},  // Count: 5, Ordinal: 1047
            {"CALCULATIONRECURRENCEPATTERNFREQUENCY", "IbA"},  // Count: 5, Ordinal: 1048
            {"CALCULATIONRECURRENCEPATTERNFREQUENCYUOM", "JbA"},  // Count: 5, Ordinal: 1049
            {"CALCULATIONRECURRENCEPATTERNSTARTTIME", "KbA"},  // Count: 5, Ordinal: 1050
            {"CALCULATIONSETDESCRIPTION", "LbA"},  // Count: 5, Ordinal: 1051
            {"CHARTGLOBALBATCHLINK", "MbA"},  // Count: 5, Ordinal: 1052
            {"CHARTGLOBALFILTERLINK", "AcA"},  // Count: 5, Ordinal: 1053
            {"CHARTMENULEVELS", "BcA"},  // Count: 5, Ordinal: 1054
            {"CHARTTAGMASK", "CcA"},  // Count: 5, Ordinal: 1055
            {"CRITERIATYPENAME", "DcA"},  // Count: 5, Ordinal: 1056
            {"CSCITATIONRESPONSEID", "EcA"},  // Count: 5, Ordinal: 1057
            {"CSRULECONFIG", "FcA"},  // Count: 5, Ordinal: 1058
            {"ENABLED", "GcA"},  // Count: 5, Ordinal: 1059
            {"EQUIPMENT_NAME", "HcA"},  // Count: 5, Ordinal: 1060
            {"F1", "IcA"},  // Count: 5, Ordinal: 1061
            {"ISBETAUSER", "JcA"},  // Count: 5, Ordinal: 1062
            {"LIMS_CRITERIA", "KcA"},  // Count: 5, Ordinal: 1063
            {"LIMSTRIGGER", "LcA"},  // Count: 5, Ordinal: 1064
            {"MODIFIEDBY", "McA"},  // Count: 5, Ordinal: 1065
            {"MODIFIEDON", "AdA"},  // Count: 5, Ordinal: 1066
            {"N'@MATERIAL_PRODUCED = '", "BdA"},  // Count: 5, Ordinal: 1067
            {"N'@MATERIAL_PRODUCED = NULL'", "CdA"},  // Count: 5, Ordinal: 1068
            {"N'@POSTING_DATE = '", "DdA"},  // Count: 5, Ordinal: 1069
            {"N'@POSTING_DATE = NULL'", "EdA"},  // Count: 5, Ordinal: 1070
            {"N'@RESOURCE = '", "FdA"},  // Count: 5, Ordinal: 1071
            {"N'@RESOURCE = NULL'", "GdA"},  // Count: 5, Ordinal: 1072
            {"N'@STOCK_TYPE = '", "HdA"},  // Count: 5, Ordinal: 1073
            {"N'@STOCK_TYPE = NULL'", "IdA"},  // Count: 5, Ordinal: 1074
            {"N'@TO_BATCH = '", "JdA"},  // Count: 5, Ordinal: 1075
            {"N'@TO_BATCH = NULL'", "KdA"},  // Count: 5, Ordinal: 1076
            {"N'@TO_MATERIAL = '", "LdA"},  // Count: 5, Ordinal: 1077
            {"N'@TO_MATERIAL = NULL'", "MdA"},  // Count: 5, Ordinal: 1078
            {"NUMBEROFSAMPLES", "AeA"},  // Count: 5, Ordinal: 1079
            {"PCBO_PRODUCTION_TRACKING", "BeA"},  // Count: 5, Ordinal: 1080
            {"PCBO_USAGE_TRACKING", "CeA"},  // Count: 5, Ordinal: 1081
            {"PI_NAME", "DeA"},  // Count: 5, Ordinal: 1082
            {"PI_SERVER", "EeA"},  // Count: 5, Ordinal: 1083
            {"PIMS_CRITERIA", "FeA"},  // Count: 5, Ordinal: 1084
            {"PIMSENVIRONMENTNAMEDVALUE", "GeA"},  // Count: 5, Ordinal: 1085
            {"PIMSPART", "HeA"},  // Count: 5, Ordinal: 1086
            {"PIMSUSERROLES", "IeA"},  // Count: 5, Ordinal: 1087
            {"PREV_BATCHID", "JeA"},  // Count: 5, Ordinal: 1088
            {"PRIMARYINFO", "KeA"},  // Count: 5, Ordinal: 1089
            {"PRODUCTID", "LeA"},  // Count: 5, Ordinal: 1090
            {"QMDEPARTMENTS", "MeA"},  // Count: 5, Ordinal: 1091
            {"QUERY_NAME", "AfA"},  // Count: 5, Ordinal: 1092
            {"RDMALARMDATE", "BfA"},  // Count: 5, Ordinal: 1093
            {"RETURN_VALUE", "CfA"},  // Count: 5, Ordinal: 1094
            {"SAF_GROUP_QUERY_XREF", "DfA"},  // Count: 5, Ordinal: 1095
            {"SAF_QUERY", "EfA"},  // Count: 5, Ordinal: 1096
            {"SAMPLE_POINT", "FfA"},  // Count: 5, Ordinal: 1097
            {"SIGMATYPE", "GfA"},  // Count: 5, Ordinal: 1098
            {"TIMEENTERED", "HfA"},  // Count: 5, Ordinal: 1099
            {"TRACE_QUEUEINFO", "IfA"},  // Count: 5, Ordinal: 1100
            {"UPDATETAGS", "JfA"},  // Count: 5, Ordinal: 1101
            {"UPPER", "KfA"},  // Count: 5, Ordinal: 1102
            {"USP_CS_INSERT_CSCAUSES", "LfA"},  // Count: 5, Ordinal: 1103
            {"UTCOFACCESS", "MfA"},  // Count: 5, Ordinal: 1104
            {"YINVADJ_INVENTORYPITAGS", "AgA"},  // Count: 5, Ordinal: 1105
            {"'%'", "FgA"},  // Count: 4, Ordinal: 1110
            {"&", "GgA"},  // Count: 4, Ordinal: 1111
            {"@ACIDCORRECTIONOFFSET", "HgA"},  // Count: 4, Ordinal: 1112
            {"@ACIDCORRECTIONSLOPE", "IgA"},  // Count: 4, Ordinal: 1113
            {"@ACIDLOWERENDPOINT", "JgA"},  // Count: 4, Ordinal: 1114
            {"@ACIDMAX", "KgA"},  // Count: 4, Ordinal: 1115
            {"@ACIDMIN", "LgA"},  // Count: 4, Ordinal: 1116
            {"@ACIDUPPERENDPOINT", "MgA"},  // Count: 4, Ordinal: 1117
            {"@ACTIONDESCRIPTION_6", "AhA"},  // Count: 4, Ordinal: 1118
            {"@ACTIONID_1", "BhA"},  // Count: 4, Ordinal: 1119
            {"@AHRCITATIONSTATUS", "ChA"},  // Count: 4, Ordinal: 1120
            {"@ALLOW_LIMIT_CHANGE", "DhA"},  // Count: 4, Ordinal: 1121
            {"@APPLICATIONNAME", "EhA"},  // Count: 4, Ordinal: 1122
            {"@APPLICATIONVERSION", "FhA"},  // Count: 4, Ordinal: 1123
            {"@AREAID_1", "GhA"},  // Count: 4, Ordinal: 1124
            {"@AREANAME", "HhA"},  // Count: 4, Ordinal: 1125
            {"@AREAQ", "IhA"},  // Count: 4, Ordinal: 1126
            {"@BASE", "JhA"},  // Count: 4, Ordinal: 1127
            {"@BASE_UNIT_NAME_2", "KhA"},  // Count: 4, Ordinal: 1128
            {"@BLENDGMN", "LhA"},  // Count: 4, Ordinal: 1129
            {"@C1", "MhA"},  // Count: 4, Ordinal: 1130
            {"@CALCULATIONRECURRENCEPATTERNDAYOFMONTH_8", "AiA"},  // Count: 4, Ordinal: 1131
            {"@CALCULATIONRECURRENCEPATTERNDAYOFWEEK_7", "BiA"},  // Count: 4, Ordinal: 1132
            {"@CALCULATIONRECURRENCEPATTERNFREQUENCY_4", "CiA"},  // Count: 4, Ordinal: 1133
            {"@CALCULATIONRECURRENCEPATTERNFREQUENCYUOM_5", "DiA"},  // Count: 4, Ordinal: 1134
            {"@CALCULATIONRECURRENCEPATTERNSTARTTIME_6", "EiA"},  // Count: 4, Ordinal: 1135
            {"@CALCULATIONSETDESCRIPTION_2", "FiA"},  // Count: 4, Ordinal: 1136
            {"@CAUSEDESCRIPTION_4", "GiA"},  // Count: 4, Ordinal: 1137
            {"@CAUSEFULLPATH_5", "HiA"},  // Count: 4, Ordinal: 1138
            {"@CHART_TYPE", "IiA"},  // Count: 4, Ordinal: 1139
            {"@CHNGTYPE", "JiA"},  // Count: 4, Ordinal: 1140
            {"@COMMENT_8", "KiA"},  // Count: 4, Ordinal: 1141
            {"@CONTROL_LIMITS", "LiA"},  // Count: 4, Ordinal: 1142
            {"@DATABASENAME", "MiA"},  // Count: 4, Ordinal: 1143
            {"@DATE_START", "AjA"},  // Count: 4, Ordinal: 1144
            {"@DATETIMEOFLASTCALCULATION_14", "BjA"},  // Count: 4, Ordinal: 1145
            {"@DAYOFTHEMONTH", "CjA"},  // Count: 4, Ordinal: 1146
            {"@DAYOFTHEWEEK", "DjA"},  // Count: 4, Ordinal: 1147
            {"@DEPT_DESCRIPTION", "EjA"},  // Count: 4, Ordinal: 1148
            {"@DISABLE", "FjA"},  // Count: 4, Ordinal: 1149
            {"@DIVISION", "GjA"},  // Count: 4, Ordinal: 1150
            {"@DIVISION_NO", "HjA"},  // Count: 4, Ordinal: 1151
            {"@ENABLED_17", "IjA"},  // Count: 4, Ordinal: 1152
            {"@ENABLESCHEDULING", "JjA"},  // Count: 4, Ordinal: 1153
            {"@ENDDATE", "KjA"},  // Count: 4, Ordinal: 1154
            {"@EVALDELAY", "LjA"},  // Count: 4, Ordinal: 1155
            {"@FRIDAY", "MjA"},  // Count: 4, Ordinal: 1156
            {"@FULLPATH", "AkA"},  // Count: 4, Ordinal: 1157
            {"@GROUP_DESCRIPTION", "BkA"},  // Count: 4, Ordinal: 1158
            {"@GROUP_NAME_8", "CkA"},  // Count: 4, Ordinal: 1159
            {"@GROUPNAME", "DkA"},  // Count: 4, Ordinal: 1160
            {"@GROUPNAME_3", "EkA"},  // Count: 4, Ordinal: 1161
            {"@HEEL", "FkA"},  // Count: 4, Ordinal: 1162
            {"@HISTORYDAYS", "GkA"},  // Count: 4, Ordinal: 1163
            {"@ID", "HkA"},  // Count: 4, Ordinal: 1164
            {"@LABOREQUIPFLAG", "IkA"},  // Count: 4, Ordinal: 1165
            {"@LCLK1", "JkA"},  // Count: 4, Ordinal: 1166
            {"@LCLK2", "KkA"},  // Count: 4, Ordinal: 1167
            {"@LCLK3", "LkA"},  // Count: 4, Ordinal: 1168
            {"@LCLK4", "MkA"},  // Count: 4, Ordinal: 1169
            {"@LEVEL_NAME", "AlA"},  // Count: 4, Ordinal: 1170
            {"@LIMITCITATION", "BlA"},  // Count: 4, Ordinal: 1171
            {"@LIMS_CRITERIA", "ClA"},  // Count: 4, Ordinal: 1172
            {"@LIMS_TRIGGER_FLAG_8", "DlA"},  // Count: 4, Ordinal: 1173
            {"@LIMSCL_TRIGGER_FLAG_10", "ElA"},  // Count: 4, Ordinal: 1174
            {"@LIMSTRIGGER_13", "FlA"},  // Count: 4, Ordinal: 1175
            {"@LINKED_UNIT_NAME_3", "GlA"},  // Count: 4, Ordinal: 1176
            {"@LIST_AREA_NAME_6", "HlA"},  // Count: 4, Ordinal: 1177
            {"@LIST_NAME_5", "IlA"},  // Count: 4, Ordinal: 1178
            {"@LISTANAME", "JlA"},  // Count: 4, Ordinal: 1179
            {"@LOGDATE", "KlA"},  // Count: 4, Ordinal: 1180
            {"@MANUAL_LCL", "LlA"},  // Count: 4, Ordinal: 1181
            {"@MANUAL_LIMITS", "MlA"},  // Count: 4, Ordinal: 1182
            {"@MANUAL_LSL", "AmA"},  // Count: 4, Ordinal: 1183
            {"@MANUAL_UCL", "BmA"},  // Count: 4, Ordinal: 1184
            {"@MANUAL_USL", "CmA"},  // Count: 4, Ordinal: 1185
            {"@MANUAL_XT", "DmA"},  // Count: 4, Ordinal: 1186
            {"@MEASURE_TYPE", "EmA"},  // Count: 4, Ordinal: 1187
            {"@MONDAY", "FmA"},  // Count: 4, Ordinal: 1188
            {"@MONTHOP1", "GmA"},  // Count: 4, Ordinal: 1189
            {"@MONTHOP2", "HmA"},  // Count: 4, Ordinal: 1190
            {"@NEWBUCKET", "ImA"},  // Count: 4, Ordinal: 1191
            {"@NEWUNITNAME", "JmA"},  // Count: 4, Ordinal: 1192
            {"@NTUSERORNTGROUPID_2", "KmA"},  // Count: 4, Ordinal: 1193
            {"@NUM_RUNS", "LmA"},  // Count: 4, Ordinal: 1194
            {"@NUMBEROFSAMPLES_12", "MmA"},  // Count: 4, Ordinal: 1195
            {"@OLDCHILDCAUSE", "AnA"},  // Count: 4, Ordinal: 1196
            {"@OLDSTRATEGYID", "BnA"},  // Count: 4, Ordinal: 1197
            {"@OPERATOR_7", "CnA"},  // Count: 4, Ordinal: 1198
            {"@PCOPERATINGSYSTEM", "DnA"},  // Count: 4, Ordinal: 1199
            {"@PIMS_CRITERIA", "EnA"},  // Count: 4, Ordinal: 1200
            {"@PIMSCLIENTVERSION", "FnA"},  // Count: 4, Ordinal: 1201
            {"@PIMSROLENAME", "GnA"},  // Count: 4, Ordinal: 1202
            {"@PIMSSDKVERSION", "HnA"},  // Count: 4, Ordinal: 1203
            {"@PIUSERID", "InA"},  // Count: 4, Ordinal: 1204
            {"@PIUSERID_3", "JnA"},  // Count: 4, Ordinal: 1205
            {"@POSTDATETIME", "KnA"},  // Count: 4, Ordinal: 1206
            {"@PROCESS_STATE_2", "LnA"},  // Count: 4, Ordinal: 1207
            {"@PROCESS_STATE_5", "MnA"},  // Count: 4, Ordinal: 1208
            {"@PROCESS_STATE_TAG_NAME_7", "AoA"},  // Count: 4, Ordinal: 1209
            {"@PRODUCT_TAG_NAME_6", "BoA"},  // Count: 4, Ordinal: 1210
            {"@PRODUCTID_15", "CoA"},  // Count: 4, Ordinal: 1211
            {"@PRODUCTION_GROUP", "DoA"},  // Count: 4, Ordinal: 1212
            {"@PRODUCTION_UNIT", "EoA"},  // Count: 4, Ordinal: 1213
            {"@PRODUCTION_UNIT_DESCRIPTION_5", "FoA"},  // Count: 4, Ordinal: 1214
            {"@PRODUCTION_UNIT_NAME_2", "GoA"},  // Count: 4, Ordinal: 1215
            {"@PROMPTFORPIPASSWORD_4", "HoA"},  // Count: 4, Ordinal: 1216
            {"@PUBLICFLAG", "IoA"},  // Count: 4, Ordinal: 1217
            {"@QUERY_NAME", "JoA"},  // Count: 4, Ordinal: 1218
            {"@RAWBATCHID", "KoA"},  // Count: 4, Ordinal: 1219
            {"@RDMACTIVESTATUS_12", "LoA"},  // Count: 4, Ordinal: 1220
            {"@RDMACTIVESTATUS_15", "MoA"},  // Count: 4, Ordinal: 1221
            {"@RDMALARMOFFSET_10", "ApA"},  // Count: 4, Ordinal: 1222
            {"@RDMALARMOFFSET_7", "BpA"},  // Count: 4, Ordinal: 1223
            {"@RDMALARMSTATUS_11", "CpA"},  // Count: 4, Ordinal: 1224
            {"@RDMALARMSTATUS_14", "DpA"},  // Count: 4, Ordinal: 1225
            {"@RDMGROUP_2", "EpA"},  // Count: 4, Ordinal: 1226
            {"@RDMGRPNAME", "FpA"},  // Count: 4, Ordinal: 1227
            {"@RDMGRPNAME_5", "GpA"},  // Count: 4, Ordinal: 1228
            {"@RDMINTERVAL_5", "HpA"},  // Count: 4, Ordinal: 1229
            {"@RDMINTERVAL_8", "IpA"},  // Count: 4, Ordinal: 1230
            {"@RDMINTERVALUNIT_6", "JpA"},  // Count: 4, Ordinal: 1231
            {"@RDMINTERVALUNIT_9", "KpA"},  // Count: 4, Ordinal: 1232
            {"@RDMSCANTIME_3", "LpA"},  // Count: 4, Ordinal: 1233
            {"@RDMSCANTIME_4", "MpA"},  // Count: 4, Ordinal: 1234
            {"@RDMSCANTIME_7", "AqA"},  // Count: 4, Ordinal: 1235
            {"@RDMSERVER", "BqA"},  // Count: 4, Ordinal: 1236
            {"@RDMSERVER_4", "CqA"},  // Count: 4, Ordinal: 1237
            {"@RDMSTARTMONTH_10", "DqA"},  // Count: 4, Ordinal: 1238
            {"@RDMSTARTMONTH_13", "EqA"},  // Count: 4, Ordinal: 1239
            {"@RDMSTARTTIME_11", "FqA"},  // Count: 4, Ordinal: 1240
            {"@RDMSTARTTIME_8", "GqA"},  // Count: 4, Ordinal: 1241
            {"@RDMSTARTWEEKDAY_12", "HqA"},  // Count: 4, Ordinal: 1242
            {"@RDMSTARTWEEKDAY_9", "IqA"},  // Count: 4, Ordinal: 1243
            {"@RDMTAGNAME_2", "JqA"},  // Count: 4, Ordinal: 1244
            {"@RDMTAGNAME_6", "KqA"},  // Count: 4, Ordinal: 1245
            {"@RECCURENCE", "LqA"},  // Count: 4, Ordinal: 1246
            {"@RECCURENCEOPTION", "MqA"},  // Count: 4, Ordinal: 1247
            {"@RECIPE", "ArA"},  // Count: 4, Ordinal: 1248
            {"@REPORT_QUARTER_2", "BrA"},  // Count: 4, Ordinal: 1249
            {"@REPORT_YEAR_3", "CrA"},  // Count: 4, Ordinal: 1250
            {"@RESOURCE_NETWORK", "DrA"},  // Count: 4, Ordinal: 1251
            {"@ROLEDESCRIPTION_2", "ErA"},  // Count: 4, Ordinal: 1252
            {"@SATURDAY", "FrA"},  // Count: 4, Ordinal: 1253
            {"@SEQUENCE_NUMBER_5", "GrA"},  // Count: 4, Ordinal: 1254
            {"@SHIFT", "HrA"},  // Count: 4, Ordinal: 1255
            {"@SHOW_2_3", "IrA"},  // Count: 4, Ordinal: 1256
            {"@SHOW_3SIG", "JrA"},  // Count: 4, Ordinal: 1257
            {"@SHOW_HISTOGRAM", "KrA"},  // Count: 4, Ordinal: 1258
            {"@SHOW_RUNS", "LrA"},  // Count: 4, Ordinal: 1259
            {"@SHOW_SPEC", "MrA"},  // Count: 4, Ordinal: 1260
            {"@SHOW_STATS", "AsA"},  // Count: 4, Ordinal: 1261
            {"@SIGMATYPE_11", "BsA"},  // Count: 4, Ordinal: 1262
            {"@SPEC_LIMIT_DATASOURCE", "CsA"},  // Count: 4, Ordinal: 1263
            {"@STARTDATE", "DsA"},  // Count: 4, Ordinal: 1264
            {"@SUBGROUP_SIZE", "EsA"},  // Count: 4, Ordinal: 1265
            {"@SUNDAY", "FsA"},  // Count: 4, Ordinal: 1266
            {"@SUPPACTION", "GsA"},  // Count: 4, Ordinal: 1267
            {"@SUPPDATA", "HsA"},  // Count: 4, Ordinal: 1268
            {"@SUPPLEMENTALDATATYPE", "IsA"},  // Count: 4, Ordinal: 1269
            {"@SYSADMINAPP", "JsA"},  // Count: 4, Ordinal: 1270
            {"@TAG_NAME", "KsA"},  // Count: 4, Ordinal: 1271
            {"@TAG_NAME_8", "LsA"},  // Count: 4, Ordinal: 1272
            {"@TARGET_FLAG", "MsA"},  // Count: 4, Ordinal: 1273
            {"@TARGET_TAG_NAME", "AtA"},  // Count: 4, Ordinal: 1274
            {"@TARGETDATE", "BtA"},  // Count: 4, Ordinal: 1275
            {"@TARGK1", "CtA"},  // Count: 4, Ordinal: 1276
            {"@TARGK2", "DtA"},  // Count: 4, Ordinal: 1277
            {"@TARGK3", "EtA"},  // Count: 4, Ordinal: 1278
            {"@TARGK4", "FtA"},  // Count: 4, Ordinal: 1279
            {"@THURSDAY", "GtA"},  // Count: 4, Ordinal: 1280
            {"@TIMESTAMP_3", "HtA"},  // Count: 4, Ordinal: 1281
            {"@TUESDAY", "ItA"},  // Count: 4, Ordinal: 1282
            {"@UCLK1", "JtA"},  // Count: 4, Ordinal: 1283
            {"@UCLK2", "KtA"},  // Count: 4, Ordinal: 1284
            {"@UCLK3", "LtA"},  // Count: 4, Ordinal: 1285
            {"@UCLK4", "MtA"},  // Count: 4, Ordinal: 1286
            {"@UPDATETAGS_16", "AuA"},  // Count: 4, Ordinal: 1287
            {"@UTCOFACCESS", "BuA"},  // Count: 4, Ordinal: 1288
            {"@V_STATUS", "CuA"},  // Count: 4, Ordinal: 1289
            {"@VALUE_CHANGED_7", "DuA"},  // Count: 4, Ordinal: 1290
            {"@VALUE10", "EuA"},  // Count: 4, Ordinal: 1291
            {"@VALUE20", "FuA"},  // Count: 4, Ordinal: 1292
            {"@VISCOSITYENDPOINT", "GuA"},  // Count: 4, Ordinal: 1293
            {"@VISCOSITYMAX", "HuA"},  // Count: 4, Ordinal: 1294
            {"@VISCOSITYMIN", "IuA"},  // Count: 4, Ordinal: 1295
            {"@VISCOSITYTEMPCOMPREF", "JuA"},  // Count: 4, Ordinal: 1296
            {"@VISCTEMPCOMPCOEFF", "KuA"},  // Count: 4, Ordinal: 1297
            {"@WEDNESDAY", "LuA"},  // Count: 4, Ordinal: 1298
            {"@WEEKCOUNT", "MuA"},  // Count: 4, Ordinal: 1299
            {"@WINUSERID", "AvA"},  // Count: 4, Ordinal: 1300
            {"@X_AXIS", "BvA"},  // Count: 4, Ordinal: 1301
            {"@Y_AXIS", "CvA"},  // Count: 4, Ordinal: 1302
            {"@ZONE_A_COLOR", "DvA"},  // Count: 4, Ordinal: 1303
            {"@ZONE_A_FLAG", "EvA"},  // Count: 4, Ordinal: 1304
            {"@ZONE_B_COLOR", "FvA"},  // Count: 4, Ordinal: 1305
            {"@ZONE_B_FLAG", "GvA"},  // Count: 4, Ordinal: 1306
            {"@ZONE_C_COLOR", "HvA"},  // Count: 4, Ordinal: 1307
            {"@ZONE_C_FLAG", "IvA"},  // Count: 4, Ordinal: 1308
            {"@ZONE_X_COLOR", "JvA"},  // Count: 4, Ordinal: 1309
            {"@ZONE_X_FLAG", "KvA"},  // Count: 4, Ordinal: 1310
            {"{FN", "LvA"},  // Count: 4, Ordinal: 1311
            {"}", "MvA"},  // Count: 4, Ordinal: 1312
            {"ACCESS", "AwA"},  // Count: 4, Ordinal: 1313
            {"ACIDCORRECTIONOFFSET", "BwA"},  // Count: 4, Ordinal: 1314
            {"ACIDCORRECTIONSLOPE", "CwA"},  // Count: 4, Ordinal: 1315
            {"ACIDLOWERENDPOINT", "DwA"},  // Count: 4, Ordinal: 1316
            {"ACIDMAX", "EwA"},  // Count: 4, Ordinal: 1317
            {"ACIDMIN", "FwA"},  // Count: 4, Ordinal: 1318
            {"ACIDUPPERENDPOINT", "GwA"},  // Count: 4, Ordinal: 1319
            {"ALLOW_LIMIT_CHANGE", "HwA"},  // Count: 4, Ordinal: 1320
            {"ANALYSIS", "IwA"},  // Count: 4, Ordinal: 1321
            {"APPACCESSLEVEL", "JwA"},  // Count: 4, Ordinal: 1322
            {"BASE_PROCESS_STATE", "KwA"},  // Count: 4, Ordinal: 1323
            {"BASE_PRODUCT_NAME", "LwA"},  // Count: 4, Ordinal: 1324
            {"CATCH", "MwA"},  // Count: 4, Ordinal: 1325
            {"CC2", "AxA"},  // Count: 4, Ordinal: 1326
            {"CHAR", "BxA"},  // Count: 4, Ordinal: 1327
            {"CHART_TYPE", "CxA"},  // Count: 4, Ordinal: 1328
            {"CHARTLEVELDATASTATUS", "DxA"},  // Count: 4, Ordinal: 1329
            {"CITATIONRESPONSEID", "ExA"},  // Count: 4, Ordinal: 1330
            {"COMPONENT", "FxA"},  // Count: 4, Ordinal: 1331
            {"CONTROL_LIMITS", "GxA"},  // Count: 4, Ordinal: 1332
            {"CRITERIA_ID", "HxA"},  // Count: 4, Ordinal: 1333
            {"CS_TO_PI", "IxA"},  // Count: 4, Ordinal: 1334
            {"CSWATCHDOGCRITERIAUPDATESTATUS", "JxA"},  // Count: 4, Ordinal: 1335
            {"CUSTOMER", "KxA"},  // Count: 4, Ordinal: 1336
            {"DAY", "LxA"},  // Count: 4, Ordinal: 1337
            {"DENNIS", "MxA"},  // Count: 4, Ordinal: 1338
            {"DEPT_DESCRIPTION", "AyA"},  // Count: 4, Ordinal: 1339
            {"DIVISION", "ByA"},  // Count: 4, Ordinal: 1340
            {"DIVISION_NO", "CyA"},  // Count: 4, Ordinal: 1341
            {"DSPCGROUPS", "DyA"},  // Count: 4, Ordinal: 1342
            {"END_TIME", "EyA"},  // Count: 4, Ordinal: 1343
            {"EVERY_N_POINTS", "FyA"},  // Count: 4, Ordinal: 1344
            {"EXCLUDE_RESAMPLES", "GyA"},  // Count: 4, Ordinal: 1345
            {"EXCLUDE_REWORKS", "HyA"},  // Count: 4, Ordinal: 1346
            {"EXCLUDE_SYNTHETICS", "IyA"},  // Count: 4, Ordinal: 1347
            {"EXTENDEDSTATUS", "JyA"},  // Count: 4, Ordinal: 1348
            {"F2", "KyA"},  // Count: 4, Ordinal: 1349
            {"FINISH_END_TIME", "LyA"},  // Count: 4, Ordinal: 1350
            {"FINISH_START_TIME", "MyA"},  // Count: 4, Ordinal: 1351
            {"FORMULA", "AzB"},  // Count: 4, Ordinal: 1352
            {"GOTO", "BzB"},  // Count: 4, Ordinal: 1353
            {"GROUP_FILTER", "CzB"},  // Count: 4, Ordinal: 1354
            {"GROUPID", "DzB"},  // Count: 4, Ordinal: 1355
            {"GSPRODUCTLIMITS", "EzB"},  // Count: 4, Ordinal: 1356
            {"INTEGER", "FzB"},  // Count: 4, Ordinal: 1357
            {"JOB_NAME", "GzB"},  // Count: 4, Ordinal: 1358
            {"L", "HzB"},  // Count: 4, Ordinal: 1359
            {"LCLK1", "IzB"},  // Count: 4, Ordinal: 1360
            {"LCLK2", "JzB"},  // Count: 4, Ordinal: 1361
            {"LCLK3", "KzB"},  // Count: 4, Ordinal: 1362
            {"LCLK4", "LzB"},  // Count: 4, Ordinal: 1363
            {"LIMS_INSTANCE", "MzB"},  // Count: 4, Ordinal: 1364
            {"LIMS_PRODUCT_VALUE", "AAB"},  // Count: 4, Ordinal: 1365
            {"LIMS_SERVER", "BAB"},  // Count: 4, Ordinal: 1366
            {"LIMS_STATE_VALUE", "CAB"},  // Count: 4, Ordinal: 1367
            {"LIMSCL_TRIGGER_FLAG", "DAB"},  // Count: 4, Ordinal: 1368
            {"LINKED_PROCESS_STATE", "EAB"},  // Count: 4, Ordinal: 1369
            {"LINKED_PRODUCT_NAME", "FAB"},  // Count: 4, Ordinal: 1370
            {"LOGIN_END_TIME", "GAB"},  // Count: 4, Ordinal: 1371
            {"LOGIN_LOCATION", "HAB"},  // Count: 4, Ordinal: 1372
            {"LOGIN_START_TIME", "IAB"},  // Count: 4, Ordinal: 1373
            {"MANUAL_LCL", "JAB"},  // Count: 4, Ordinal: 1374
            {"MANUAL_LIMITS", "KAB"},  // Count: 4, Ordinal: 1375
            {"MANUAL_LSL", "LAB"},  // Count: 4, Ordinal: 1376
            {"MANUAL_UCL", "MAB"},  // Count: 4, Ordinal: 1377
            {"MANUAL_USL", "ABB"},  // Count: 4, Ordinal: 1378
            {"MANUAL_XT", "BBB"},  // Count: 4, Ordinal: 1379
            {"N'@TO_PLANT = '", "CBB"},  // Count: 4, Ordinal: 1380
            {"N'@TO_PLANT = NULL'", "DBB"},  // Count: 4, Ordinal: 1381
            {"N'@TO_SLOC = '", "EBB"},  // Count: 4, Ordinal: 1382
            {"N'@TO_SLOC = NULL'", "FBB"},  // Count: 4, Ordinal: 1383
            {"N'GROUP'", "GBB"},  // Count: 4, Ordinal: 1384
            {"NOLOCK", "HBB"},  // Count: 4, Ordinal: 1385
            {"NOW", "IBB"},  // Count: 4, Ordinal: 1386
            {"NUM_RUNS", "JBB"},  // Count: 4, Ordinal: 1387
            {"NUMBER_OF_PARTS", "KBB"},  // Count: 4, Ordinal: 1388
            {"ORDERED_LIST_LIST", "LBB"},  // Count: 4, Ordinal: 1389
            {"PART_NUMBER", "MBB"},  // Count: 4, Ordinal: 1390
            {"PCBASEUNIT", "ACB"},  // Count: 4, Ordinal: 1391
            {"PI", "BCB"},  // Count: 4, Ordinal: 1392
            {"PIMSAPPLICATIONNAME", "CCB"},  // Count: 4, Ordinal: 1393
            {"PIMSAPPLICATIONS", "DCB"},  // Count: 4, Ordinal: 1394
            {"PIMSENVIRONMENT", "ECB"},  // Count: 4, Ordinal: 1395
            {"PIMSGROUPS", "FCB"},  // Count: 4, Ordinal: 1396
            {"PM_NUMBER", "GCB"},  // Count: 4, Ordinal: 1397
            {"QUALITY_STATUS", "HCB"},  // Count: 4, Ordinal: 1398
            {"RDMCONTINUOUSALARM", "ICB"},  // Count: 4, Ordinal: 1399
            {"RDMMONITORARCHIVEDELTA", "JCB"},  // Count: 4, Ordinal: 1400
            {"RDMMONITORARCHIVEFLAG", "KCB"},  // Count: 4, Ordinal: 1401
            {"RDMSERVERDESC", "LCB"},  // Count: 4, Ordinal: 1402
            {"READ_ONLY", "MCB"},  // Count: 4, Ordinal: 1403
            {"RELATIVE_END", "ADB"},  // Count: 4, Ordinal: 1404
            {"RELATIVE_FINISH_FLAG", "BDB"},  // Count: 4, Ordinal: 1405
            {"RELATIVE_LOGIN_FLAG", "CDB"},  // Count: 4, Ordinal: 1406
            {"RELATIVE_START", "DDB"},  // Count: 4, Ordinal: 1407
            {"RELATIVE_TIME_FLAG", "EDB"},  // Count: 4, Ordinal: 1408
            {"RETRYCOUNT", "FDB"},  // Count: 4, Ordinal: 1409
            {"SAF_CHART", "GDB"},  // Count: 4, Ordinal: 1410
            {"SAF_GROUP", "HDB"},  // Count: 4, Ordinal: 1411
            {"SAF_LIMS_CRITERIA", "IDB"},  // Count: 4, Ordinal: 1412
            {"SAF_PIMS_CRITERIA", "JDB"},  // Count: 4, Ordinal: 1413
            {"SAMPLE_CODE", "KDB"},  // Count: 4, Ordinal: 1414
            {"SAMPLE_TYPE", "LDB"},  // Count: 4, Ordinal: 1415
            {"SHOW_2_3", "MDB"},  // Count: 4, Ordinal: 1416
            {"SHOW_3SIG", "AEB"},  // Count: 4, Ordinal: 1417
            {"SHOW_HISTOGRAM", "BEB"},  // Count: 4, Ordinal: 1418
            {"SHOW_RUNS", "CEB"},  // Count: 4, Ordinal: 1419
            {"SHOW_SPEC", "DEB"},  // Count: 4, Ordinal: 1420
            {"SHOW_STATS", "EEB"},  // Count: 4, Ordinal: 1421
            {"SIGMAHISTORYTAG", "FEB"},  // Count: 4, Ordinal: 1422
            {"SPOKENLANGUAGE", "GEB"},  // Count: 4, Ordinal: 1423
            {"START_TIME", "HEB"},  // Count: 4, Ordinal: 1424
            {"STAT", "IEB"},  // Count: 4, Ordinal: 1425
            {"STATE", "JEB"},  // Count: 4, Ordinal: 1426
            {"SUBGROUP_SIZE", "KEB"},  // Count: 4, Ordinal: 1427
            {"TARGK1", "LEB"},  // Count: 4, Ordinal: 1428
            {"TARGK2", "MEB"},  // Count: 4, Ordinal: 1429
            {"TARGK3", "AFB"},  // Count: 4, Ordinal: 1430
            {"TARGK4", "BFB"},  // Count: 4, Ordinal: 1431
            {"TEST_SCHEDULE", "CFB"},  // Count: 4, Ordinal: 1432
            {"TOOLTIP", "DFB"},  // Count: 4, Ordinal: 1433
            {"TOP", "EFB"},  // Count: 4, Ordinal: 1434
            {"TRY", "FFB"},  // Count: 4, Ordinal: 1435
            {"UCLK1", "GFB"},  // Count: 4, Ordinal: 1436
            {"UCLK2", "HFB"},  // Count: 4, Ordinal: 1437
            {"UCLK3", "IFB"},  // Count: 4, Ordinal: 1438
            {"UCLK4", "JFB"},  // Count: 4, Ordinal: 1439
            {"UNION", "KFB"},  // Count: 4, Ordinal: 1440
            {"UNIT_FILTER", "LFB"},  // Count: 4, Ordinal: 1441
            {"VISCOSITYENDPOINT", "MFB"},  // Count: 4, Ordinal: 1442
            {"VISCOSITYMAX", "AGB"},  // Count: 4, Ordinal: 1443
            {"VISCOSITYMIN", "BGB"},  // Count: 4, Ordinal: 1444
            {"VISCOSITYTEMPCOMPCOEFFICIENT", "CGB"},  // Count: 4, Ordinal: 1445
            {"VISCOSITYTEMPCOMPREF", "DGB"},  // Count: 4, Ordinal: 1446
            {"X_AXIS", "EGB"},  // Count: 4, Ordinal: 1447
            {"Y_AXIS", "FGB"},  // Count: 4, Ordinal: 1448
            {"YINVADJ_PIINVENTORY", "GGB"},  // Count: 4, Ordinal: 1449
            {"ZONE_A_COLOR", "HGB"},  // Count: 4, Ordinal: 1450
            {"ZONE_A_FLAG", "IGB"},  // Count: 4, Ordinal: 1451
            {"ZONE_B_COLOR", "JGB"},  // Count: 4, Ordinal: 1452
            {"ZONE_B_FLAG", "KGB"},  // Count: 4, Ordinal: 1453
            {"ZONE_C_COLOR", "LGB"},  // Count: 4, Ordinal: 1454
            {"ZONE_C_FLAG", "MGB"},  // Count: 4, Ordinal: 1455
            {"ZONE_X_COLOR", "AHB"},  // Count: 4, Ordinal: 1456
            {"ZONE_X_FLAG", "BHB"},  // Count: 4, Ordinal: 1457
            {"@ACTIVITY1", "GHB"},  // Count: 3, Ordinal: 1462
            {"@ACTIVITY1_UNITS", "HHB"},  // Count: 3, Ordinal: 1463
            {"@ACTIVITY1FINISHED", "IHB"},  // Count: 3, Ordinal: 1464
            {"@ACTIVITY2", "JHB"},  // Count: 3, Ordinal: 1465
            {"@ACTIVITY2_UNITS", "KHB"},  // Count: 3, Ordinal: 1466
            {"@ACTIVITY2FINISHED", "LHB"},  // Count: 3, Ordinal: 1467
            {"@ACTIVITY3", "MHB"},  // Count: 3, Ordinal: 1468
            {"@ACTIVITY3_UNITS", "AIB"},  // Count: 3, Ordinal: 1469
            {"@ACTIVITY3FINISHED", "BIB"},  // Count: 3, Ordinal: 1470
            {"@ACTIVITY4FINISHED", "CIB"},  // Count: 3, Ordinal: 1471
            {"@ACTIVITY4SIGNED", "DIB"},  // Count: 3, Ordinal: 1472
            {"@ACTIVITY5", "EIB"},  // Count: 3, Ordinal: 1473
            {"@ACTIVITY5_UNITS", "FIB"},  // Count: 3, Ordinal: 1474
            {"@ACTIVITY5FINISHED", "GIB"},  // Count: 3, Ordinal: 1475
            {"@ACTIVITY6FINISHED", "HIB"},  // Count: 3, Ordinal: 1476
            {"@ARCHIVEDAYS", "IIB"},  // Count: 3, Ordinal: 1477
            {"@BATCH_CHECK_SW", "JIB"},  // Count: 3, Ordinal: 1478
            {"@BOLNUMBER", "KIB"},  // Count: 3, Ordinal: 1479
            {"@BSTATUS", "LIB"},  // Count: 3, Ordinal: 1480
            {"@CLASSIFICATION", "MIB"},  // Count: 3, Ordinal: 1481
            {"@CLEANAREA", "AJB"},  // Count: 3, Ordinal: 1482
            {"@CLEAR_RESERVATIONS", "BJB"},  // Count: 3, Ordinal: 1483
            {"@COMPARISON_OPERATOR", "CJB"},  // Count: 3, Ordinal: 1484
            {"@CONFIRMATION_SHORT_TEXT", "DJB"},  // Count: 3, Ordinal: 1485
            {"@CONFIRMATIONSHORTTEXT", "EJB"},  // Count: 3, Ordinal: 1486
            {"@CONTAINER_ID", "FJB"},  // Count: 3, Ordinal: 1487
            {"@CPY_BATCH", "GJB"},  // Count: 3, Ordinal: 1488
            {"@CPY_MATERIAL", "HJB"},  // Count: 3, Ordinal: 1489
            {"@CREATE_ONLY", "IJB"},  // Count: 3, Ordinal: 1490
            {"@DELIVERY", "JJB"},  // Count: 3, Ordinal: 1491
            {"@DOCUMENTURL_3", "KJB"},  // Count: 3, Ordinal: 1492
            {"@DOCUMENTURL_4", "LJB"},  // Count: 3, Ordinal: 1493
            {"@DURATION", "MJB"},  // Count: 3, Ordinal: 1494
            {"@END_DATE", "AKB"},  // Count: 3, Ordinal: 1495
            {"@EVENTDATE", "BKB"},  // Count: 3, Ordinal: 1496
            {"@EVENTTIME", "CKB"},  // Count: 3, Ordinal: 1497
            {"@EXECCMD", "DKB"},  // Count: 3, Ordinal: 1498
            {"@FINAL_CONFIRMATION", "EKB"},  // Count: 3, Ordinal: 1499
            {"@FINAL_ISSUE", "FKB"},  // Count: 3, Ordinal: 1500
            {"@FINALISSUE", "GKB"},  // Count: 3, Ordinal: 1501
            {"@FRM_BATCH", "HKB"},  // Count: 3, Ordinal: 1502
            {"@FRM_MATERIAL", "IKB"},  // Count: 3, Ordinal: 1503
            {"@FROMRECIPEID", "JKB"},  // Count: 3, Ordinal: 1504
            {"@GENERATECONSUMPTIONBIT", "KKB"},  // Count: 3, Ordinal: 1505
            {"@GMNDESCRIPTION", "LKB"},  // Count: 3, Ordinal: 1506
            {"@HEADERTEXT", "MKB"},  // Count: 3, Ordinal: 1507
            {"@INPARM", "ALB"},  // Count: 3, Ordinal: 1508
            {"@INSPECTION_ORIGIN", "BLB"},  // Count: 3, Ordinal: 1509
            {"@INSPECTION_SHORT_TEXT", "CLB"},  // Count: 3, Ordinal: 1510
            {"@INSPLOT", "DLB"},  // Count: 3, Ordinal: 1511
            {"@INSPLOTORIGIN", "ELB"},  // Count: 3, Ordinal: 1512
            {"@INSPORIGIN", "FLB"},  // Count: 3, Ordinal: 1513
            {"@INSPTYPE", "GLB"},  // Count: 3, Ordinal: 1514
            {"@LIFNR", "HLB"},  // Count: 3, Ordinal: 1515
            {"@LIST_AREA", "ILB"},  // Count: 3, Ordinal: 1516
            {"@LIST_AREA_NAME", "JLB"},  // Count: 3, Ordinal: 1517
            {"@LIST_NAME", "KLB"},  // Count: 3, Ordinal: 1518
            {"@LOC", "LLB"},  // Count: 3, Ordinal: 1519
            {"@LOGICAL_OPERATOR", "MLB"},  // Count: 3, Ordinal: 1520
            {"@MAINT_WINDOW", "AMB"},  // Count: 3, Ordinal: 1521
            {"@MATERIAL_DOCUMENT", "BMB"},  // Count: 3, Ordinal: 1522
            {"@MATERIAL_DOCUMENT_YEAR", "CMB"},  // Count: 3, Ordinal: 1523
            {"@MATERIAL_ID", "DMB"},  // Count: 3, Ordinal: 1524
            {"@MAXSIZE", "EMB"},  // Count: 3, Ordinal: 1525
            {"@MD013_INV_STATUS", "FMB"},  // Count: 3, Ordinal: 1526
            {"@MF_BATCH", "GMB"},  // Count: 3, Ordinal: 1527
            {"@MF_MATERIAL", "HMB"},  // Count: 3, Ordinal: 1528
            {"@NAME21", "IMB"},  // Count: 3, Ordinal: 1529
            {"@NAME22", "JMB"},  // Count: 3, Ordinal: 1530
            {"@NAME23", "KMB"},  // Count: 3, Ordinal: 1531
            {"@NAME24", "LMB"},  // Count: 3, Ordinal: 1532
            {"@NAME25", "MMB"},  // Count: 3, Ordinal: 1533
            {"@NAME26", "ANB"},  // Count: 3, Ordinal: 1534
            {"@NAME27", "BNB"},  // Count: 3, Ordinal: 1535
            {"@NAME28", "CNB"},  // Count: 3, Ordinal: 1536
            {"@NAME29", "DNB"},  // Count: 3, Ordinal: 1537
            {"@NAME30", "ENB"},  // Count: 3, Ordinal: 1538
            {"@NEW_POSTING_DATE", "FNB"},  // Count: 3, Ordinal: 1539
            {"@NEWPOSTTIME", "GNB"},  // Count: 3, Ordinal: 1540
            {"@NEWSTRATEGYID", "HNB"},  // Count: 3, Ordinal: 1541
            {"@NEXTCHILDID", "INB"},  // Count: 3, Ordinal: 1542
            {"@NO_OF_REMINDER_MAILS_3", "JNB"},  // Count: 3, Ordinal: 1543
            {"@NUMBEROFPARTS", "KNB"},  // Count: 3, Ordinal: 1544
            {"@OLDCAUSEID", "LNB"},  // Count: 3, Ordinal: 1545
            {"@OLDVALUE", "MNB"},  // Count: 3, Ordinal: 1546
            {"@OPERATION_PRM", "AOB"},  // Count: 3, Ordinal: 1547
            {"@OUTAGE_END_DATE", "BOB"},  // Count: 3, Ordinal: 1548
            {"@OUTAGE_END_TIME", "COB"},  // Count: 3, Ordinal: 1549
            {"@OUTAGE_START_DATE", "DOB"},  // Count: 3, Ordinal: 1550
            {"@OUTAGE_START_TIME", "EOB"},  // Count: 3, Ordinal: 1551
            {"@OUTPARM", "FOB"},  // Count: 3, Ordinal: 1552
            {"@PERFORM_QA01", "GOB"},  // Count: 3, Ordinal: 1553
            {"@PERFORM_WHEN_INV_EXISTS", "HOB"},  // Count: 3, Ordinal: 1554
            {"@PHASE_RESOURCE", "IOB"},  // Count: 3, Ordinal: 1555
            {"@PHASE_RESOURCE_PRM", "JOB"},  // Count: 3, Ordinal: 1556
            {"@PICONS_FROM_MATL_BATCH", "KOB"},  // Count: 3, Ordinal: 1557
            {"@PIDISPLAYURL_4", "LOB"},  // Count: 3, Ordinal: 1558
            {"@PIDISPLAYURL_5", "MOB"},  // Count: 3, Ordinal: 1559
            {"@PIMSDB", "APB"},  // Count: 3, Ordinal: 1560
            {"@PISERVER_WHERE", "BPB"},  // Count: 3, Ordinal: 1561
            {"@PITAGDESC", "CPB"},  // Count: 3, Ordinal: 1562
            {"@PITAGNAME_WHERE", "DPB"},  // Count: 3, Ordinal: 1563
            {"@PKG_BATCH", "EPB"},  // Count: 3, Ordinal: 1564
            {"@PKG_ID", "FPB"},  // Count: 3, Ordinal: 1565
            {"@PKG_MATERIAL", "GPB"},  // Count: 3, Ordinal: 1566
            {"@PKG_STATUS", "HPB"},  // Count: 3, Ordinal: 1567
            {"@PKG_TYP", "IPB"},  // Count: 3, Ordinal: 1568
            {"@PLANT_ID_PRM", "JPB"},  // Count: 3, Ordinal: 1569
            {"@PLANTID", "KPB"},  // Count: 3, Ordinal: 1570
            {"@PO_NUMBER", "LPB"},  // Count: 3, Ordinal: 1571
            {"@POCOMPLETE", "MPB"},  // Count: 3, Ordinal: 1572
            {"@PONM01", "AQB"},  // Count: 3, Ordinal: 1573
            {"@PONM02", "BQB"},  // Count: 3, Ordinal: 1574
            {"@PONM03", "CQB"},  // Count: 3, Ordinal: 1575
            {"@PONM04", "DQB"},  // Count: 3, Ordinal: 1576
            {"@PONM05", "EQB"},  // Count: 3, Ordinal: 1577
            {"@PONM06", "FQB"},  // Count: 3, Ordinal: 1578
            {"@PONM07", "GQB"},  // Count: 3, Ordinal: 1579
            {"@PONM08", "HQB"},  // Count: 3, Ordinal: 1580
            {"@PONM09", "IQB"},  // Count: 3, Ordinal: 1581
            {"@PONM10", "JQB"},  // Count: 3, Ordinal: 1582
            {"@PONM11", "KQB"},  // Count: 3, Ordinal: 1583
            {"@PONM12", "LQB"},  // Count: 3, Ordinal: 1584
            {"@PONM13", "MQB"},  // Count: 3, Ordinal: 1585
            {"@PONM14", "ARB"},  // Count: 3, Ordinal: 1586
            {"@PONM15", "BRB"},  // Count: 3, Ordinal: 1587
            {"@PONM16", "CRB"},  // Count: 3, Ordinal: 1588
            {"@PONM17", "DRB"},  // Count: 3, Ordinal: 1589
            {"@PONM18", "ERB"},  // Count: 3, Ordinal: 1590
            {"@PONM19", "FRB"},  // Count: 3, Ordinal: 1591
            {"@PONM20", "GRB"},  // Count: 3, Ordinal: 1592
            {"@PONM21", "HRB"},  // Count: 3, Ordinal: 1593
            {"@PONM22", "IRB"},  // Count: 3, Ordinal: 1594
            {"@PONM23", "JRB"},  // Count: 3, Ordinal: 1595
            {"@PONM24", "KRB"},  // Count: 3, Ordinal: 1596
            {"@PONM25", "LRB"},  // Count: 3, Ordinal: 1597
            {"@PONM26", "MRB"},  // Count: 3, Ordinal: 1598
            {"@PONM27", "ASB"},  // Count: 3, Ordinal: 1599
            {"@PONM28", "BSB"},  // Count: 3, Ordinal: 1600
            {"@PONM29", "CSB"},  // Count: 3, Ordinal: 1601
            {"@PONM30", "DSB"},  // Count: 3, Ordinal: 1602
            {"@POSTTIME", "ESB"},  // Count: 3, Ordinal: 1603
            {"@PPPI_PLANT_OF_BATCH", "FSB"},  // Count: 3, Ordinal: 1604
            {"@PREVCHILDID", "GSB"},  // Count: 3, Ordinal: 1605
            {"@PROCESS_ORDER_PRM", "HSB"},  // Count: 3, Ordinal: 1606
            {"@PROD_SCHED_PROF", "ISB"},  // Count: 3, Ordinal: 1607
            {"@PRODUCT_BATCH_ID", "JSB"},  // Count: 3, Ordinal: 1608
            {"@PRODUCTION_DATE", "KSB"},  // Count: 3, Ordinal: 1609
            {"@PRODUCTION_VERSION", "LSB"},  // Count: 3, Ordinal: 1610
            {"@REFNUMBER", "MSB"},  // Count: 3, Ordinal: 1611
            {"@REPORTQUARTER_4", "ATB"},  // Count: 3, Ordinal: 1612
            {"@REPORTYEAR_5", "BTB"},  // Count: 3, Ordinal: 1613
            {"@SAMPLEDATE", "CTB"},  // Count: 3, Ordinal: 1614
            {"@SCHEDULED", "DTB"},  // Count: 3, Ordinal: 1615
            {"@SCHEDULING_TYPE", "ETB"},  // Count: 3, Ordinal: 1616
            {"@SCRAPTOCONFIRM", "FTB"},  // Count: 3, Ordinal: 1617
            {"@SECONDARY_RESOURCE", "GTB"},  // Count: 3, Ordinal: 1618
            {"@SLOC", "HTB"},  // Count: 3, Ordinal: 1619
            {"@SPCGRAPHPITAG_5", "ITB"},  // Count: 3, Ordinal: 1620
            {"@SPCGRAPHPITAG_6", "JTB"},  // Count: 3, Ordinal: 1621
            {"@SQCSTATUS", "KTB"},  // Count: 3, Ordinal: 1622
            {"@SQLTEXT", "LTB"},  // Count: 3, Ordinal: 1623
            {"@START_DATE", "MTB"},  // Count: 3, Ordinal: 1624
            {"@STOCKTYPE", "AUB"},  // Count: 3, Ordinal: 1625
            {"@STRATEGY", "BUB"},  // Count: 3, Ordinal: 1626
            {"@SUPPLIER", "CUB"},  // Count: 3, Ordinal: 1627
            {"@SYSTEM_ID", "DUB"},  // Count: 3, Ordinal: 1628
            {"@TAGMASK", "EUB"},  // Count: 3, Ordinal: 1629
            {"@TIME_4", "FUB"},  // Count: 3, Ordinal: 1630
            {"@TO_STORAGE_LOCATION", "GUB"},  // Count: 3, Ordinal: 1631
            {"@TORECIPEID", "HUB"},  // Count: 3, Ordinal: 1632
            {"@TRACEPATHALL", "IUB"},  // Count: 3, Ordinal: 1633
            {"@UNITS", "JUB"},  // Count: 3, Ordinal: 1634
            {"@USG_BLOCKED_ALLOWED", "KUB"},  // Count: 3, Ordinal: 1635
            {"@V_POS", "LUB"},  // Count: 3, Ordinal: 1636
            {"@V_TEMP", "MUB"},  // Count: 3, Ordinal: 1637
            {"@VALUE21", "AVB"},  // Count: 3, Ordinal: 1638
            {"@VALUE22", "BVB"},  // Count: 3, Ordinal: 1639
            {"@VALUE23", "CVB"},  // Count: 3, Ordinal: 1640
            {"@VALUE24", "DVB"},  // Count: 3, Ordinal: 1641
            {"@VALUE25", "EVB"},  // Count: 3, Ordinal: 1642
            {"@VALUE26", "FVB"},  // Count: 3, Ordinal: 1643
            {"@VALUE27", "GVB"},  // Count: 3, Ordinal: 1644
            {"@VALUE28", "HVB"},  // Count: 3, Ordinal: 1645
            {"@VALUE29", "IVB"},  // Count: 3, Ordinal: 1646
            {"@VALUE30", "JVB"},  // Count: 3, Ordinal: 1647
            {"@VENDOR", "KVB"},  // Count: 3, Ordinal: 1648
            {"@WATCHDOGCRITERIAUPDATESTATUS", "LVB"},  // Count: 3, Ordinal: 1649
            {"-1'", "MVB"},  // Count: 3, Ordinal: 1650
            {"ACTUALSHIPDATE", "AWB"},  // Count: 3, Ordinal: 1651
            {"APP", "BWB"},  // Count: 3, Ordinal: 1652
            {"ARC_CSCITATIONS", "CWB"},  // Count: 3, Ordinal: 1653
            {"BATCH_LINK", "DWB"},  // Count: 3, Ordinal: 1654
            {"BATCH_TYPE", "EWB"},  // Count: 3, Ordinal: 1655
            {"BINARY", "FWB"},  // Count: 3, Ordinal: 1656
            {"BULK_PKG", "GWB"},  // Count: 3, Ordinal: 1657
            {"CGTYPE", "HWB"},  // Count: 3, Ordinal: 1658
            {"CHARTBATCHTYPEEXPRESSIONS", "IWB"},  // Count: 3, Ordinal: 1659
            {"CS_TO_PIGROUP", "JWB"},  // Count: 3, Ordinal: 1660
            {"CSACTIONINSTRUCTIONS", "KWB"},  // Count: 3, Ordinal: 1661
            {"CSCAUSEINSTRUCTIONS", "LWB"},  // Count: 3, Ordinal: 1662
            {"CSOTHERACTIONFORBID", "MWB"},  // Count: 3, Ordinal: 1663
            {"CSOTHERCAUSEFORBID", "AXB"},  // Count: 3, Ordinal: 1664
            {"CSTYPES", "BXB"},  // Count: 3, Ordinal: 1665
            {"DAYOFTHEMONTH", "CXB"},  // Count: 3, Ordinal: 1666
            {"DAYOFTHEWEEK", "DXB"},  // Count: 3, Ordinal: 1667
            {"DB_LOCATION", "EXB"},  // Count: 3, Ordinal: 1668
            {"DSPCTAGCHANGEHISTORY", "FXB"},  // Count: 3, Ordinal: 1669
            {"ENABLESCHEDULING", "GXB"},  // Count: 3, Ordinal: 1670
            {"ENDTIME", "HXB"},  // Count: 3, Ordinal: 1671
            {"EXECUTE", "IXB"},  // Count: 3, Ordinal: 1672
            {"FILTER_NUMBER", "JXB"},  // Count: 3, Ordinal: 1673
            {"FILTER1_LINK", "KXB"},  // Count: 3, Ordinal: 1674
            {"FILTER1_NAME", "LXB"},  // Count: 3, Ordinal: 1675
            {"FILTER1_TYPE", "MXB"},  // Count: 3, Ordinal: 1676
            {"FILTER2_LINK", "AYB"},  // Count: 3, Ordinal: 1677
            {"FILTER2_NAME", "BYB"},  // Count: 3, Ordinal: 1678
            {"FILTER2_TYPE", "CYB"},  // Count: 3, Ordinal: 1679
            {"FRIDAY", "DYB"},  // Count: 3, Ordinal: 1680
            {"HEEL", "EYB"},  // Count: 3, Ordinal: 1681
            {"HISTORYDAYS", "FYB"},  // Count: 3, Ordinal: 1682
            {"ID", "GYB"},  // Count: 3, Ordinal: 1683
            {"ITEMID", "HYB"},  // Count: 3, Ordinal: 1684
            {"LIMITS_GROUP", "IYB"},  // Count: 3, Ordinal: 1685
            {"MONDAY", "JYB"},  // Count: 3, Ordinal: 1686
            {"MONTH", "KYB"},  // Count: 3, Ordinal: 1687
            {"MONTHOP1", "LYB"},  // Count: 3, Ordinal: 1688
            {"MONTHOP2", "MYB"},  // Count: 3, Ordinal: 1689
            {"N''''", "AZB"},  // Count: 3, Ordinal: 1690
            {"N'@COST_CENTER = '", "BZB"},  // Count: 3, Ordinal: 1691
            {"N'@COST_CENTER = NULL'", "CZB"},  // Count: 3, Ordinal: 1692
            {"N'@DELIVERY_COMPLETE = '", "DZB"},  // Count: 3, Ordinal: 1693
            {"N'@DELIVERY_COMPLETE = NULL'", "EZB"},  // Count: 3, Ordinal: 1694
            {"N'@DELV02 = '", "FZB"},  // Count: 3, Ordinal: 1695
            {"N'@ENDEVENTDATE = '", "GZB"},  // Count: 3, Ordinal: 1696
            {"N'@ENDEVENTDATE = NULL'", "HZB"},  // Count: 3, Ordinal: 1697
            {"N'@ENDEVENTTIME = '", "IZB"},  // Count: 3, Ordinal: 1698
            {"N'@ENDEVENTTIME = NULL'", "JZB"},  // Count: 3, Ordinal: 1699
            {"N'@QUANTITY = '", "KZB"},  // Count: 3, Ordinal: 1700
            {"N'@QUANTITY = NULL'", "LZB"},  // Count: 3, Ordinal: 1701
            {"N'@QUANTITY_TYPE = '", "MZB"},  // Count: 3, Ordinal: 1702
            {"N'@QUANTITY_TYPE = NULL'", "AaB"},  // Count: 3, Ordinal: 1703
            {"N'@REASON_FOR_VARIANCE = '", "BaB"},  // Count: 3, Ordinal: 1704
            {"N'@REASON_FOR_VARIANCE = NULL'", "CaB"},  // Count: 3, Ordinal: 1705
            {"N'@RESERVATION = '", "DaB"},  // Count: 3, Ordinal: 1706
            {"N'@RESERVATION = NULL'", "EaB"},  // Count: 3, Ordinal: 1707
            {"N'@RESERVATION_ITEM = '", "FaB"},  // Count: 3, Ordinal: 1708
            {"N'@RESERVATION_ITEM = NULL'", "GaB"},  // Count: 3, Ordinal: 1709
            {"N'@STARTEVENTDATE = '", "HaB"},  // Count: 3, Ordinal: 1710
            {"N'@STARTEVENTDATE = NULL'", "IaB"},  // Count: 3, Ordinal: 1711
            {"N'@STARTEVENTTIME = '", "JaB"},  // Count: 3, Ordinal: 1712
            {"N'@STARTEVENTTIME = NULL'", "KaB"},  // Count: 3, Ordinal: 1713
            {"N'@TMATERIAL = '", "LaB"},  // Count: 3, Ordinal: 1714
            {"N'@TMATERIAL = NULL'", "MaB"},  // Count: 3, Ordinal: 1715
            {"N'@YIELD_TO_CONFIRM = '", "AbB"},  // Count: 3, Ordinal: 1716
            {"N'@YIELD_TO_CONFIRM = NULL'", "BbB"},  // Count: 3, Ordinal: 1717
            {"N'LISTAREA'", "CbB"},  // Count: 3, Ordinal: 1718
            {"N'N'", "DbB"},  // Count: 3, Ordinal: 1719
            {"N'U'", "EbB"},  // Count: 3, Ordinal: 1720
            {"N'Y_BT_CR'", "FbB"},  // Count: 3, Ordinal: 1721
            {"ON_ERROR1:", "GbB"},  // Count: 3, Ordinal: 1722
            {"ORDERED_LIST", "HbB"},  // Count: 3, Ordinal: 1723
            {"PART", "IbB"},  // Count: 3, Ordinal: 1724
            {"PCBO_RESOURCE_GMN", "JbB"},  // Count: 3, Ordinal: 1725
            {"PIMSACCOUNTING", "KbB"},  // Count: 3, Ordinal: 1726
            {"PIMSADMIN", "LbB"},  // Count: 3, Ordinal: 1727
            {"PIMSCONTACTUID", "MbB"},  // Count: 3, Ordinal: 1728
            {"PIMSENVIRONMENTNAMEDVALUECHANGEHISTORY", "AcB"},  // Count: 3, Ordinal: 1729
            {"PIMSPROCORDER", "BcB"},  // Count: 3, Ordinal: 1730
            {"PIMSUOM", "CcB"},  // Count: 3, Ordinal: 1731
            {"PIMSWEBDISPLAYTEXT", "DcB"},  // Count: 3, Ordinal: 1732
            {"PIMSWEBRELATIVEURL", "EcB"},  // Count: 3, Ordinal: 1733
            {"PRINTING", "FcB"},  // Count: 3, Ordinal: 1734
            {"PROC", "GcB"},  // Count: 3, Ordinal: 1735
            {"PROCESS_ORDER", "HcB"},  // Count: 3, Ordinal: 1736
            {"RAW_GMN", "IcB"},  // Count: 3, Ordinal: 1737
            {"RDMALARMHISTORY", "JcB"},  // Count: 3, Ordinal: 1738
            {"RDMTAG", "KcB"},  // Count: 3, Ordinal: 1739
            {"RECCURENCE", "LcB"},  // Count: 3, Ordinal: 1740
            {"RECCURENCEOPTION", "McB"},  // Count: 3, Ordinal: 1741
            {"REPLACE", "AdB"},  // Count: 3, Ordinal: 1742
            {"REPLACE_STRING", "BdB"},  // Count: 3, Ordinal: 1743
            {"REPLACE_STRING_BATCH", "CdB"},  // Count: 3, Ordinal: 1744
            {"REPLACE_STRING_FILTER1", "DdB"},  // Count: 3, Ordinal: 1745
            {"REPLACE_STRING_FILTER2", "EdB"},  // Count: 3, Ordinal: 1746
            {"SATURDAY", "FdB"},  // Count: 3, Ordinal: 1747
            {"SECPIMSADMIN", "GdB"},  // Count: 3, Ordinal: 1748
            {"SECPIMSAPPLICATION", "HdB"},  // Count: 3, Ordinal: 1749
            {"SP_DELETE_POINT_AND_TAG", "IdB"},  // Count: 3, Ordinal: 1750
            {"SP_TRACE_SETFILTER", "JdB"},  // Count: 3, Ordinal: 1751
            {"SP_TRACE_SETSTATUS", "KdB"},  // Count: 3, Ordinal: 1752
            {"SPEC_LIMIT_DATASOURCE", "LdB"},  // Count: 3, Ordinal: 1753
            {"SQCDERIVESTATUS", "MdB"},  // Count: 3, Ordinal: 1754
            {"STARTTIME", "AeB"},  // Count: 3, Ordinal: 1755
            {"STORAGE_LOCATION", "BeB"},  // Count: 3, Ordinal: 1756
            {"SUNDAY", "CeB"},  // Count: 3, Ordinal: 1757
            {"SYSADMINAPP", "DeB"},  // Count: 3, Ordinal: 1758
            {"SYSNAME", "EeB"},  // Count: 3, Ordinal: 1759
            {"THURSDAY", "FeB"},  // Count: 3, Ordinal: 1760
            {"TUESDAY", "GeB"},  // Count: 3, Ordinal: 1761
            {"TYPE_TEXT_ALIAS_TAG_MASK", "HeB"},  // Count: 3, Ordinal: 1762
            {"TYPES", "IeB"},  // Count: 3, Ordinal: 1763
            {"UPGRADE_LOCATION", "JeB"},  // Count: 3, Ordinal: 1764
            {"USP_CS_INSERT_CSACTIONMASTER", "KeB"},  // Count: 3, Ordinal: 1765
            {"USP_CS_INSERT_CSACTIONTAGS", "LeB"},  // Count: 3, Ordinal: 1766
            {"USP_CS_INSERT_CSCAUSEACTION", "MeB"},  // Count: 3, Ordinal: 1767
            {"VERSION_NUMBER", "AfB"},  // Count: 3, Ordinal: 1768
            {"WATCHDOGCRITERIAUPDATEPENDING", "BfB"},  // Count: 3, Ordinal: 1769
            {"WEDNESDAY", "CfB"},  // Count: 3, Ordinal: 1770
            {"WEEKCOUNT", "DfB"},  // Count: 3, Ordinal: 1771
            {"YEAR", "EfB"},  // Count: 3, Ordinal: 1772
            {"-12", "FfB"},  // Count: 2, Ordinal: 1773
            {"-2", "GfB"},  // Count: 2, Ordinal: 1774
            {"1095", "IfB"},  // Count: 2, Ordinal: 1776
            {"1460", "JfB"},  // Count: 2, Ordinal: 1777
            {"''''", "KfB"},  // Count: 2, Ordinal: 1778
            {"@ACTIONID_4", "LfB"},  // Count: 2, Ordinal: 1779
            {"@ACTIONTAKEN", "MfB"},  // Count: 2, Ordinal: 1780
            {"@ACTIVEFLAG", "AgB"},  // Count: 2, Ordinal: 1781
            {"@ACTUALSHIPDATE", "BgB"},  // Count: 2, Ordinal: 1782
            {"@AFTERDATE", "CgB"},  // Count: 2, Ordinal: 1783
            {"@ALLOWCHANGINGLIMITS", "DgB"},  // Count: 2, Ordinal: 1784
            {"@APP", "EgB"},  // Count: 2, Ordinal: 1785
            {"@APPACCESSLEVEL_3", "FgB"},  // Count: 2, Ordinal: 1786
            {"@APPACCESSLEVEL_5", "GgB"},  // Count: 2, Ordinal: 1787
            {"@APPLICATION", "HgB"},  // Count: 2, Ordinal: 1788
            {"@APPLICATION_NAME_2", "IgB"},  // Count: 2, Ordinal: 1789
            {"@APPLICATIONNAME_2", "JgB"},  // Count: 2, Ordinal: 1790
            {"@APPNAME_4", "KgB"},  // Count: 2, Ordinal: 1791
            {"@ARCHIVE", "LgB"},  // Count: 2, Ordinal: 1792
            {"@AREANAME_2", "MgB"},  // Count: 2, Ordinal: 1793
            {"@AREANAME_4", "AhB"},  // Count: 2, Ordinal: 1794
            {"@BALERNUMBER", "BhB"},  // Count: 2, Ordinal: 1795
            {"@BASE_PROCESS_STATE_8", "ChB"},  // Count: 2, Ordinal: 1796
            {"@BASE_PRODUCT_NAME_6", "DhB"},  // Count: 2, Ordinal: 1797
            {"@BASE_TAG_1", "EhB"},  // Count: 2, Ordinal: 1798
            {"@BASE_TAG_NAME_4", "FhB"},  // Count: 2, Ordinal: 1799
            {"@BASE_UNIT_NAME", "GhB"},  // Count: 2, Ordinal: 1800
            {"@BASE_UNIT_NAME_1", "HhB"},  // Count: 2, Ordinal: 1801
            {"@BEGINDATETIME", "IhB"},  // Count: 2, Ordinal: 1802
            {"@BLENDBATCHID", "JhB"},  // Count: 2, Ordinal: 1803
            {"@CALCULATIONSETNAME_3", "KhB"},  // Count: 2, Ordinal: 1804
            {"@CALCULATIONSETNAME_4", "LhB"},  // Count: 2, Ordinal: 1805
            {"@CAUSEID_3", "MhB"},  // Count: 2, Ordinal: 1806
            {"@CAUSEID_4", "AiB"},  // Count: 2, Ordinal: 1807
            {"@CHARACTER", "BiB"},  // Count: 2, Ordinal: 1808
            {"@CHILDCAUSEID_4", "CiB"},  // Count: 2, Ordinal: 1809
            {"@COMPARE1", "DiB"},  // Count: 2, Ordinal: 1810
            {"@COMPARE2", "EiB"},  // Count: 2, Ordinal: 1811
            {"@CONTAINERID", "FiB"},  // Count: 2, Ordinal: 1812
            {"@CONTROLLIMITS", "GiB"},  // Count: 2, Ordinal: 1813
            {"@CREWNUMBER", "HiB"},  // Count: 2, Ordinal: 1814
            {"@CURRENTVALUE", "IiB"},  // Count: 2, Ordinal: 1815
            {"@CUSTOMERSHIPTONAME", "JiB"},  // Count: 2, Ordinal: 1816
            {"@CUSTOMERSHIPTONUMBER", "KiB"},  // Count: 2, Ordinal: 1817
            {"@CUSTOMERSOLDTONAME", "LiB"},  // Count: 2, Ordinal: 1818
            {"@CUSTOMERSOLDTONUMBER", "MiB"},  // Count: 2, Ordinal: 1819
            {"@DATETIME_6", "AjB"},  // Count: 2, Ordinal: 1820
            {"@DATETIMEOFLASTCALCULATION_2", "BjB"},  // Count: 2, Ordinal: 1821
            {"@DAYSDIFF", "CjB"},  // Count: 2, Ordinal: 1822
            {"@DB_LOCATION_3", "DjB"},  // Count: 2, Ordinal: 1823
            {"@DB_LOCATION_4", "EjB"},  // Count: 2, Ordinal: 1824
            {"@DELIVERYITEMNUMBER", "FjB"},  // Count: 2, Ordinal: 1825
            {"@DELIVERYNUMBER", "GjB"},  // Count: 2, Ordinal: 1826
            {"@DESC", "HjB"},  // Count: 2, Ordinal: 1827
            {"@ENDDATETIME", "IjB"},  // Count: 2, Ordinal: 1828
            {"@EQUIPMENT_2", "JjB"},  // Count: 2, Ordinal: 1829
            {"@EQUIPMENT_3", "KjB"},  // Count: 2, Ordinal: 1830
            {"@EQUIPMENT_NAME_4", "LjB"},  // Count: 2, Ordinal: 1831
            {"@EXCLUDE_BLANKBATCH", "MjB"},  // Count: 2, Ordinal: 1832
            {"@EXTENDEDSTATUS", "AkB"},  // Count: 2, Ordinal: 1833
            {"@FILTERTAG1", "BkB"},  // Count: 2, Ordinal: 1834
            {"@FILTERTAG2", "CkB"},  // Count: 2, Ordinal: 1835
            {"@FILTERVALUE1", "DkB"},  // Count: 2, Ordinal: 1836
            {"@FILTERVALUE2", "EkB"},  // Count: 2, Ordinal: 1837
            {"@FLAG_2", "FkB"},  // Count: 2, Ordinal: 1838
            {"@GCCOMPONENT_4", "GkB"},  // Count: 2, Ordinal: 1839
            {"@GCMETHOD_3", "HkB"},  // Count: 2, Ordinal: 1840
            {"@GENCITATION", "IkB"},  // Count: 2, Ordinal: 1841
            {"@GROUP_1", "JkB"},  // Count: 2, Ordinal: 1842
            {"@GROUP_DESCRIPTION_3", "KkB"},  // Count: 2, Ordinal: 1843
            {"@GROUP_DESCRIPTION_4", "LkB"},  // Count: 2, Ordinal: 1844
            {"@GROUP_PROMPT", "MkB"},  // Count: 2, Ordinal: 1845
            {"@GROUP_PROMPT_2", "AlB"},  // Count: 2, Ordinal: 1846
            {"@GROUP_PROMPT_3", "BlB"},  // Count: 2, Ordinal: 1847
            {"@GROUPDESCRIPTION_2", "ClB"},  // Count: 2, Ordinal: 1848
            {"@GROUPDESCRIPTION_3", "DlB"},  // Count: 2, Ordinal: 1849
            {"@GROUPID_2", "ElB"},  // Count: 2, Ordinal: 1850
            {"@GROUPNAME_4", "FlB"},  // Count: 2, Ordinal: 1851
            {"@GROUPSET", "GlB"},  // Count: 2, Ordinal: 1852
            {"@HEEL_IDENTIFIER", "HlB"},  // Count: 2, Ordinal: 1853
            {"@HISTLIMITSINALARM", "IlB"},  // Count: 2, Ordinal: 1854
            {"@IN_I_QUARTER", "JlB"},  // Count: 2, Ordinal: 1855
            {"@IN_I_YEAR", "KlB"},  // Count: 2, Ordinal: 1856
            {"@IN_S_SERVERNAME", "LlB"},  // Count: 2, Ordinal: 1857
            {"@INALARM", "MlB"},  // Count: 2, Ordinal: 1858
            {"@INPUTDATA", "AmB"},  // Count: 2, Ordinal: 1859
            {"@INSERT", "BmB"},  // Count: 2, Ordinal: 1860
            {"@INVQUANTITY", "CmB"},  // Count: 2, Ordinal: 1861
            {"@ISBETAUSER_2", "DmB"},  // Count: 2, Ordinal: 1862
            {"@ISBETAUSER_3", "EmB"},  // Count: 2, Ordinal: 1863
            {"@ISNTGROUP_3", "FmB"},  // Count: 2, Ordinal: 1864
            {"@ISNTGROUP_4", "GmB"},  // Count: 2, Ordinal: 1865
            {"@ISROOTCAUSE", "HmB"},  // Count: 2, Ordinal: 1866
            {"@LASTPARTIDENTIFIER", "ImB"},  // Count: 2, Ordinal: 1867
            {"@LC", "JmB"},  // Count: 2, Ordinal: 1868
            {"@LIMIT", "KmB"},  // Count: 2, Ordinal: 1869
            {"@LIMS_PRODUCT_7", "LmB"},  // Count: 2, Ordinal: 1870
            {"@LIMS_PRODUCT_OR_STATE_FLAG_11", "MmB"},  // Count: 2, Ordinal: 1871
            {"@LIMS_STATE_8", "AnB"},  // Count: 2, Ordinal: 1872
            {"@LINKED_PROCESS_STATE_9", "BnB"},  // Count: 2, Ordinal: 1873
            {"@LINKED_PRODUCT_NAME_7", "CnB"},  // Count: 2, Ordinal: 1874
            {"@LINKED_TAG_2", "DnB"},  // Count: 2, Ordinal: 1875
            {"@LINKED_TAG_NAME_5", "EnB"},  // Count: 2, Ordinal: 1876
            {"@LIST_AREA_DESCRIPTION", "FnB"},  // Count: 2, Ordinal: 1877
            {"@LIST_AREA_DESCRIPTION_4", "GnB"},  // Count: 2, Ordinal: 1878
            {"@LIST_AREA_DESCRIPTION_6", "HnB"},  // Count: 2, Ordinal: 1879
            {"@LIST_AREA_NAME_7", "InB"},  // Count: 2, Ordinal: 1880
            {"@LIST_AREA_PROMPT", "JnB"},  // Count: 2, Ordinal: 1881
            {"@LIST_AREA_PROMPT_3", "KnB"},  // Count: 2, Ordinal: 1882
            {"@LIST_AREA_PROMPT_5", "LnB"},  // Count: 2, Ordinal: 1883
            {"@LIST_DESCRIPTION", "MnB"},  // Count: 2, Ordinal: 1884
            {"@LIST_DESCRIPTION_4", "AoB"},  // Count: 2, Ordinal: 1885
            {"@LIST_DESCRIPTION_7", "BoB"},  // Count: 2, Ordinal: 1886
            {"@LIST_NAME_4", "CoB"},  // Count: 2, Ordinal: 1887
            {"@LIST_NAME_6", "DoB"},  // Count: 2, Ordinal: 1888
            {"@LISTAREANAME", "EoB"},  // Count: 2, Ordinal: 1889
            {"@MANUALLIMITS", "FoB"},  // Count: 2, Ordinal: 1890
            {"@MASKLEVEL", "GoB"},  // Count: 2, Ordinal: 1891
            {"@MODIFIEDBY", "HoB"},  // Count: 2, Ordinal: 1892
            {"@MODIFIEDBY_4", "IoB"},  // Count: 2, Ordinal: 1893
            {"@MOVEMENTTYPE", "JoB"},  // Count: 2, Ordinal: 1894
            {"@MYRULEID", "KoB"},  // Count: 2, Ordinal: 1895
            {"@NEWACTIONDATE", "LoB"},  // Count: 2, Ordinal: 1896
            {"@NEWCOMMENT", "MoB"},  // Count: 2, Ordinal: 1897
            {"@NEWDESCRIPTION", "ApB"},  // Count: 2, Ordinal: 1898
            {"@NEWFULL_PATH", "BpB"},  // Count: 2, Ordinal: 1899
            {"@NEWFULL_PATH_SEGMENT", "CpB"},  // Count: 2, Ordinal: 1900
            {"@NEWLCL_5", "DpB"},  // Count: 2, Ordinal: 1901
            {"@NEWLCL_8", "EpB"},  // Count: 2, Ordinal: 1902
            {"@NEWLEVEL_CONTENT", "FpB"},  // Count: 2, Ordinal: 1903
            {"@NEWLEVELTYPE", "GpB"},  // Count: 2, Ordinal: 1904
            {"@NEWOPERATOR", "HpB"},  // Count: 2, Ordinal: 1905
            {"@NEWPRODUCT", "IpB"},  // Count: 2, Ordinal: 1906
            {"@NEWSTATE", "JpB"},  // Count: 2, Ordinal: 1907
            {"@NEWTAGNAME", "KpB"},  // Count: 2, Ordinal: 1908
            {"@NEWTARG_6", "LpB"},  // Count: 2, Ordinal: 1909
            {"@NEWTARG_9", "MpB"},  // Count: 2, Ordinal: 1910
            {"@NEWTYPE", "AqB"},  // Count: 2, Ordinal: 1911
            {"@NEWUCL_4", "BqB"},  // Count: 2, Ordinal: 1912
            {"@NEWUCL_7", "CqB"},  // Count: 2, Ordinal: 1913
            {"@OLDTAGNAME", "DqB"},  // Count: 2, Ordinal: 1914
            {"@OPERAND", "EqB"},  // Count: 2, Ordinal: 1915
            {"@OPTIONS", "FqB"},  // Count: 2, Ordinal: 1916
            {"@ORDER_LIST_DESCRIPTION_4", "GqB"},  // Count: 2, Ordinal: 1917
            {"@ORDER_LIST_DESCRIPTION_7", "HqB"},  // Count: 2, Ordinal: 1918
            {"@ORDER_LIST_NAME_4", "IqB"},  // Count: 2, Ordinal: 1919
            {"@ORDER_LIST_NAME_5", "JqB"},  // Count: 2, Ordinal: 1920
            {"@ORDERNUMBER", "KqB"},  // Count: 2, Ordinal: 1921
            {"@ORIGINAL_LEVEL_NUMBER", "LqB"},  // Count: 2, Ordinal: 1922
            {"@PARENTCAUSEID_3", "MqB"},  // Count: 2, Ordinal: 1923
            {"@PART_NUMBER", "ArB"},  // Count: 2, Ordinal: 1924
            {"@PC", "BrB"},  // Count: 2, Ordinal: 1925
            {"@PERSIST", "CrB"},  // Count: 2, Ordinal: 1926
            {"@PHASE_ID", "DrB"},  // Count: 2, Ordinal: 1927
            {"@PI_DESCRIPTION_3", "ErB"},  // Count: 2, Ordinal: 1928
            {"@PI_DESCRIPTION_4", "FrB"},  // Count: 2, Ordinal: 1929
            {"@PI_NAME_2", "GrB"},  // Count: 2, Ordinal: 1930
            {"@PI_NODE_NAME_2", "HrB"},  // Count: 2, Ordinal: 1931
            {"@PI_NODE_NAME_3", "IrB"},  // Count: 2, Ordinal: 1932
            {"@PI_TAG_NAME_9", "JrB"},  // Count: 2, Ordinal: 1933
            {"@PIMSCONTACT_1UID_2", "KrB"},  // Count: 2, Ordinal: 1934
            {"@PIMSCONTACTUID", "LrB"},  // Count: 2, Ordinal: 1935
            {"@PIMSDEVDB", "MrB"},  // Count: 2, Ordinal: 1936
            {"@PIMSSDKRESPONSETIME", "AsB"},  // Count: 2, Ordinal: 1937
            {"@PIMSSERVERDB", "BsB"},  // Count: 2, Ordinal: 1938
            {"@PIMSWEBDISPLAYTEXT_2", "CsB"},  // Count: 2, Ordinal: 1939
            {"@PIMSWEBDISPLAYTEXT_3", "DsB"},  // Count: 2, Ordinal: 1940
            {"@PIMSWEBRELATIVEURL_3", "EsB"},  // Count: 2, Ordinal: 1941
            {"@PIMSWEBRELATIVEURL_4", "FsB"},  // Count: 2, Ordinal: 1942
            {"@PISERVER_3", "GsB"},  // Count: 2, Ordinal: 1943
            {"@PISERVER_5", "HsB"},  // Count: 2, Ordinal: 1944
            {"@PKG_COUNT", "IsB"},  // Count: 2, Ordinal: 1945
            {"@PKG_SIZE", "JsB"},  // Count: 2, Ordinal: 1946
            {"@POINTID", "KsB"},  // Count: 2, Ordinal: 1947
            {"@POSITION", "LsB"},  // Count: 2, Ordinal: 1948
            {"@PREC_3", "MsB"},  // Count: 2, Ordinal: 1949
            {"@PREC_4", "AtB"},  // Count: 2, Ordinal: 1950
            {"@PREPIMSDEVDB", "BtB"},  // Count: 2, Ordinal: 1951
            {"@PREPIMSSERVER", "CtB"},  // Count: 2, Ordinal: 1952
            {"@PREV_BATCHID", "DtB"},  // Count: 2, Ordinal: 1953
            {"@PREVPATH", "EtB"},  // Count: 2, Ordinal: 1954
            {"@PRIMARYINFO", "FtB"},  // Count: 2, Ordinal: 1955
            {"@PRINT_QUEUE_5", "GtB"},  // Count: 2, Ordinal: 1956
            {"@PROCCHNGDATETIME", "HtB"},  // Count: 2, Ordinal: 1957
            {"@PROCESS_MESSAGE", "ItB"},  // Count: 2, Ordinal: 1958
            {"@PROD_UNIT", "JtB"},  // Count: 2, Ordinal: 1959
            {"@PRODUCT_NAME_3", "KtB"},  // Count: 2, Ordinal: 1960
            {"@PRODUCT_NAME_4", "LtB"},  // Count: 2, Ordinal: 1961
            {"@PRODUCT_NAME_6", "MtB"},  // Count: 2, Ordinal: 1962
            {"@PRODUCTION_UNIT_NAME_6", "AuB"},  // Count: 2, Ordinal: 1963
            {"@PRODUCTION_UNIT_NAME_7", "BuB"},  // Count: 2, Ordinal: 1964
            {"@QTYSHIPPEDGROSS", "CuB"},  // Count: 2, Ordinal: 1965
            {"@QTYSHIPPEDNET", "DuB"},  // Count: 2, Ordinal: 1966
            {"@QTYSHIPPEDUOM", "EuB"},  // Count: 2, Ordinal: 1967
            {"@QUARTER", "FuB"},  // Count: 2, Ordinal: 1968
            {"@RDMACTIVE_6", "GuB"},  // Count: 2, Ordinal: 1969
            {"@RDMACTIVE_8", "HuB"},  // Count: 2, Ordinal: 1970
            {"@RDMALARM_7", "IuB"},  // Count: 2, Ordinal: 1971
            {"@RDMALARM_9", "JuB"},  // Count: 2, Ordinal: 1972
            {"@RDMALARMDATE", "KuB"},  // Count: 2, Ordinal: 1973
            {"@RDMALARMDATE_2", "LuB"},  // Count: 2, Ordinal: 1974
            {"@RDMALARMSTATUS_4", "MuB"},  // Count: 2, Ordinal: 1975
            {"@RDMALTAG", "AvB"},  // Count: 2, Ordinal: 1976
            {"@RDMALTAG_4", "BvB"},  // Count: 2, Ordinal: 1977
            {"@RDMALTAG_6", "CvB"},  // Count: 2, Ordinal: 1978
            {"@RDMGRPDESC_3", "DvB"},  // Count: 2, Ordinal: 1979
            {"@RDMGRPDESC_5", "EvB"},  // Count: 2, Ordinal: 1980
            {"@RDMMONITORARCHIVEDELTA_14", "FvB"},  // Count: 2, Ordinal: 1981
            {"@RDMMONITORARCHIVEDELTA_17", "GvB"},  // Count: 2, Ordinal: 1982
            {"@RDMMONITORARCHIVEFLAG_13", "HvB"},  // Count: 2, Ordinal: 1983
            {"@RDMMONITORARCHIVEFLAG_16", "IvB"},  // Count: 2, Ordinal: 1984
            {"@RDMSERVER_2", "JvB"},  // Count: 2, Ordinal: 1985
            {"@RDMSERVERDESC_2", "KvB"},  // Count: 2, Ordinal: 1986
            {"@RDMSERVERDESC_3", "LvB"},  // Count: 2, Ordinal: 1987
            {"@RDMSTATUS_2", "MvB"},  // Count: 2, Ordinal: 1988
            {"@RDMSTATUS_5", "AwB"},  // Count: 2, Ordinal: 1989
            {"@RDMSTATUS_7", "BwB"},  // Count: 2, Ordinal: 1990
            {"@RDMTAG", "CwB"},  // Count: 2, Ordinal: 1991
            {"@RDMTAG_3", "DwB"},  // Count: 2, Ordinal: 1992
            {"@RDMTAGNAME", "EwB"},  // Count: 2, Ordinal: 1993
            {"@RDMVALIDTAGVALUE", "FwB"},  // Count: 2, Ordinal: 1994
            {"@REFACTIONID", "GwB"},  // Count: 2, Ordinal: 1995
            {"@REFDATECREATED", "HwB"},  // Count: 2, Ordinal: 1996
            {"@REFRESETTIME", "IwB"},  // Count: 2, Ordinal: 1997
            {"@RETRYCOUNT", "JwB"},  // Count: 2, Ordinal: 1998
            {"@RETVAL<>0", "KwB"},  // Count: 2, Ordinal: 1999
            {"@RMDTAGNAME_2", "LwB"},  // Count: 2, Ordinal: 2000
            {"@ROLENAME_2", "MwB"},  // Count: 2, Ordinal: 2001
            {"@RULE", "AxB"},  // Count: 2, Ordinal: 2002
            {"@RULE_ID", "BxB"},  // Count: 2, Ordinal: 2003
            {"@RULEDESC", "CxB"},  // Count: 2, Ordinal: 2004
            {"@RUNNUMBER", "DxB"},  // Count: 2, Ordinal: 2005
            {"@S_VAL_PR_DB_SUPPORT", "ExB"},  // Count: 2, Ordinal: 2006
            {"@SAMPLEID", "FxB"},  // Count: 2, Ordinal: 2007
            {"@SAMPLEPOINT", "GxB"},  // Count: 2, Ordinal: 2008
            {"@SCHEDULEDSHIPDATE", "HxB"},  // Count: 2, Ordinal: 2009
            {"@SECONDARYINFO", "IxB"},  // Count: 2, Ordinal: 2010
            {"@SERVER", "JxB"},  // Count: 2, Ordinal: 2011
            {"@SGLLCVALUE", "KxB"},  // Count: 2, Ordinal: 2012
            {"@SGLLSVALUE", "LxB"},  // Count: 2, Ordinal: 2013
            {"@SGLUCVALUE", "MxB"},  // Count: 2, Ordinal: 2014
            {"@SGLUSVALUE", "AyB"},  // Count: 2, Ordinal: 2015
            {"@SGLXTVALUE", "ByB"},  // Count: 2, Ordinal: 2016
            {"@SHIPPINGPOINT", "CyB"},  // Count: 2, Ordinal: 2017
            {"@SHIPTOCITY", "DyB"},  // Count: 2, Ordinal: 2018
            {"@SHOW2OF3ALM", "EyB"},  // Count: 2, Ordinal: 2019
            {"@SHOW3SIGALM", "FyB"},  // Count: 2, Ordinal: 2020
            {"@SHOWADDSTATS", "GyB"},  // Count: 2, Ordinal: 2021
            {"@SHOWRUNALM", "HyB"},  // Count: 2, Ordinal: 2022
            {"@SHOWSPECLIM", "IyB"},  // Count: 2, Ordinal: 2023
            {"@SHOWSTATS", "JyB"},  // Count: 2, Ordinal: 2024
            {"@SIGMAHISTORYTAG_3", "KyB"},  // Count: 2, Ordinal: 2025
            {"@SIGMAHISTORYTAG_5", "LyB"},  // Count: 2, Ordinal: 2026
            {"@SINCETIME", "MyB"},  // Count: 2, Ordinal: 2027
            {"@SOLDTOCITY", "AzC"},  // Count: 2, Ordinal: 2028
            {"@SPC_GRAPHTYPE", "BzC"},  // Count: 2, Ordinal: 2029
            {"@SPOKENLANGUAGE_4", "CzC"},  // Count: 2, Ordinal: 2030
            {"@SPOKENLANGUAGE_5", "DzC"},  // Count: 2, Ordinal: 2031
            {"@STATI", "EzC"},  // Count: 2, Ordinal: 2032
            {"@STORAGELOCATION", "FzC"},  // Count: 2, Ordinal: 2033
            {"@STRATEGYID_3", "GzC"},  // Count: 2, Ordinal: 2034
            {"@SUBGROUPSIZE", "HzC"},  // Count: 2, Ordinal: 2035
            {"@SYSTEM_NAME", "IzC"},  // Count: 2, Ordinal: 2036
            {"@TAG_NAME_2", "JzC"},  // Count: 2, Ordinal: 2037
            {"@TAGNAME_3", "KzC"},  // Count: 2, Ordinal: 2038
            {"@TAGNAME_5", "LzC"},  // Count: 2, Ordinal: 2039
            {"@TAGNAME_6", "MzC"},  // Count: 2, Ordinal: 2040
            {"@TAGSWEREUPDATED_10", "AAC"},  // Count: 2, Ordinal: 2041
            {"@TAGSWEREUPDATED_7", "BAC"},  // Count: 2, Ordinal: 2042
            {"@TOTALINVENTORYQUANTITY", "CAC"},  // Count: 2, Ordinal: 2043
            {"@TRANSDATE", "DAC"},  // Count: 2, Ordinal: 2044
            {"@UC", "EAC"},  // Count: 2, Ordinal: 2045
            {"@UPDATEFLAG", "FAC"},  // Count: 2, Ordinal: 2046
            {"@UPGRADE_LOCATION_4", "GAC"},  // Count: 2, Ordinal: 2047
            {"@UPGRADE_LOCATION_5", "HAC"},  // Count: 2, Ordinal: 2048
            {"@USERACTION", "IAC"},  // Count: 2, Ordinal: 2049
            {"@USERNAME", "JAC"},  // Count: 2, Ordinal: 2050
            {"@VALUE_10", "KAC"},  // Count: 2, Ordinal: 2051
            {"@VERSION_NUMBER_2", "LAC"},  // Count: 2, Ordinal: 2052
            {"@VERSION_NUMBER_3", "MAC"},  // Count: 2, Ordinal: 2053
            {"@WINDOWS_UID", "ABC"},  // Count: 2, Ordinal: 2054
            {"@XAXIS", "BBC"},  // Count: 2, Ordinal: 2055
            {"@XT", "CBC"},  // Count: 2, Ordinal: 2056
            {"@YAXIS", "DBC"},  // Count: 2, Ordinal: 2057
            {"@YEAR", "EBC"},  // Count: 2, Ordinal: 2058
            {"@ZONEA", "FBC"},  // Count: 2, Ordinal: 2059
            {"@ZONEACOLOR", "GBC"},  // Count: 2, Ordinal: 2060
            {"@ZONEB", "HBC"},  // Count: 2, Ordinal: 2061
            {"@ZONEBCOLOR", "IBC"},  // Count: 2, Ordinal: 2062
            {"@ZONEC", "JBC"},  // Count: 2, Ordinal: 2063
            {"@ZONECCOLOR", "KBC"},  // Count: 2, Ordinal: 2064
            {"@ZONEX", "LBC"},  // Count: 2, Ordinal: 2065
            {"@ZONEXCOLOR", "MBC"},  // Count: 2, Ordinal: 2066
            {"0'", "ACC"},  // Count: 2, Ordinal: 2067
            {"1'", "BCC"},  // Count: 2, Ordinal: 2068
            {"'1'", "CCC"},  // Count: 2, Ordinal: 2069
            {"ACTMASTER", "DCC"},  // Count: 2, Ordinal: 2070
            {"ALARMSTATE", "ECC"},  // Count: 2, Ordinal: 2071
            {"ALARMSTATEID", "FCC"},  // Count: 2, Ordinal: 2072
            {"APPLICATIONVERSION", "GCC"},  // Count: 2, Ordinal: 2073
            {"ARC_CSCITATIONCRITERIA", "HCC"},  // Count: 2, Ordinal: 2074
            {"ARC_CSCITATIONLOG", "ICC"},  // Count: 2, Ordinal: 2075
            {"BATCHCHARACTER", "JCC"},  // Count: 2, Ordinal: 2076
            {"BATCHPOSITION", "KCC"},  // Count: 2, Ordinal: 2077
            {"BATCHTYPE", "LCC"},  // Count: 2, Ordinal: 2078
            {"BETA", "MCC"},  // Count: 2, Ordinal: 2079
            {"BLEND_GMN", "ADC"},  // Count: 2, Ordinal: 2080
            {"BUCKET", "BDC"},  // Count: 2, Ordinal: 2081
            {"CAUSACT", "CDC"},  // Count: 2, Ordinal: 2082
            {"CITATIONS'", "DDC"},  // Count: 2, Ordinal: 2083
            {"CLOSETIME", "EDC"},  // Count: 2, Ordinal: 2084
            {"COMPARE1", "FDC"},  // Count: 2, Ordinal: 2085
            {"COMPARE2", "GDC"},  // Count: 2, Ordinal: 2086
            {"CS_GETRULES", "HDC"},  // Count: 2, Ordinal: 2087
            {"CSACTIONTAGSLOG", "IDC"},  // Count: 2, Ordinal: 2088
            {"CSALARMSTATES", "JDC"},  // Count: 2, Ordinal: 2089
            {"DATEDIFF", "KDC"},  // Count: 2, Ordinal: 2090
            {"DCS_BATCH_MODULE", "LDC"},  // Count: 2, Ordinal: 2091
            {"DEST_FILE", "MDC"},  // Count: 2, Ordinal: 2092
            {"EMN_SP_SEND_YDELV", "AEC"},  // Count: 2, Ordinal: 2093
            {"EVENTDATETIME", "BEC"},  // Count: 2, Ordinal: 2094
            {"EXCLUDE_BLANKBATCH", "CEC"},  // Count: 2, Ordinal: 2095
            {"FILTERTAG1", "DEC"},  // Count: 2, Ordinal: 2096
            {"FILTERTAG2", "EEC"},  // Count: 2, Ordinal: 2097
            {"FILTERVALUE1", "FEC"},  // Count: 2, Ordinal: 2098
            {"FILTERVALUE2", "GEC"},  // Count: 2, Ordinal: 2099
            {"GMNDESCRIPTION", "HEC"},  // Count: 2, Ordinal: 2100
            {"GROUPDESCRIPTION", "IEC"},  // Count: 2, Ordinal: 2101
            {"HANDLE", "JEC"},  // Count: 2, Ordinal: 2102
            {"INSTRUCTIONS", "KEC"},  // Count: 2, Ordinal: 2103
            {"LABOR_EQUIP_FLAG", "LEC"},  // Count: 2, Ordinal: 2104
            {"LEVEL_NAME", "MEC"},  // Count: 2, Ordinal: 2105
            {"LIMS_PRODUCT_OR_STATE_FLAG", "AFC"},  // Count: 2, Ordinal: 2106
            {"LOG_DATE", "BFC"},  // Count: 2, Ordinal: 2107
            {"LOGICALID", "CFC"},  // Count: 2, Ordinal: 2108
            {"'MASK'", "DFC"},  // Count: 2, Ordinal: 2109
            {"MASTER", "EFC"},  // Count: 2, Ordinal: 2110
            {"N'@ACTIVITY4 = '", "FFC"},  // Count: 2, Ordinal: 2111
            {"N'@ACTIVITY4 = NULL'", "GFC"},  // Count: 2, Ordinal: 2112
            {"N'@ACTIVITY4_UNITS = '", "HFC"},  // Count: 2, Ordinal: 2113
            {"N'@ACTIVITY4_UNITS = NULL'", "IFC"},  // Count: 2, Ordinal: 2114
            {"N'@ACTIVITY6 = '", "JFC"},  // Count: 2, Ordinal: 2115
            {"N'@ACTIVITY6 = NULL'", "KFC"},  // Count: 2, Ordinal: 2116
            {"N'@ACTIVITY6_UNITS = '", "LFC"},  // Count: 2, Ordinal: 2117
            {"N'@ACTIVITY6_UNITS = NULL'", "MFC"},  // Count: 2, Ordinal: 2118
            {"N'@ADDBATCHES = '", "AGC"},  // Count: 2, Ordinal: 2119
            {"N'@ADDBATCHES = NULL'", "BGC"},  // Count: 2, Ordinal: 2120
            {"N'@ADDBD01 = '", "CGC"},  // Count: 2, Ordinal: 2121
            {"N'@ADDBD01 = NULL'", "DGC"},  // Count: 2, Ordinal: 2122
            {"N'@ADDBD02 = '", "EGC"},  // Count: 2, Ordinal: 2123
            {"N'@ADDBD02 = NULL'", "FGC"},  // Count: 2, Ordinal: 2124
            {"N'@ADDBD03 = '", "GGC"},  // Count: 2, Ordinal: 2125
            {"N'@ADDBD03 = NULL'", "HGC"},  // Count: 2, Ordinal: 2126
            {"N'@ADDBD04 = '", "IGC"},  // Count: 2, Ordinal: 2127
            {"N'@ADDBD04 = NULL'", "JGC"},  // Count: 2, Ordinal: 2128
            {"N'@ADDBD05 = '", "KGC"},  // Count: 2, Ordinal: 2129
            {"N'@ADDBD05 = NULL'", "LGC"},  // Count: 2, Ordinal: 2130
            {"N'@ADDBD06 = '", "MGC"},  // Count: 2, Ordinal: 2131
            {"N'@ADDBD06 = NULL'", "AHC"},  // Count: 2, Ordinal: 2132
            {"N'@ADDBD07 = '", "BHC"},  // Count: 2, Ordinal: 2133
            {"N'@ADDBD07 = NULL'", "CHC"},  // Count: 2, Ordinal: 2134
            {"N'@ADDBD08 = '", "DHC"},  // Count: 2, Ordinal: 2135
            {"N'@ADDBD08 = NULL'", "EHC"},  // Count: 2, Ordinal: 2136
            {"N'@ADDBD09 = '", "FHC"},  // Count: 2, Ordinal: 2137
            {"N'@ADDBD09 = NULL'", "GHC"},  // Count: 2, Ordinal: 2138
            {"N'@ADDBD10 = '", "HHC"},  // Count: 2, Ordinal: 2139
            {"N'@ADDBD10 = NULL'", "IHC"},  // Count: 2, Ordinal: 2140
            {"N'@ADDBD11 = '", "JHC"},  // Count: 2, Ordinal: 2141
            {"N'@ADDBD11 = NULL'", "KHC"},  // Count: 2, Ordinal: 2142
            {"N'@ADDBD12 = '", "LHC"},  // Count: 2, Ordinal: 2143
            {"N'@ADDBD12 = NULL'", "MHC"},  // Count: 2, Ordinal: 2144
            {"N'@ADDBD13 = '", "AIC"},  // Count: 2, Ordinal: 2145
            {"N'@ADDBD13 = NULL'", "BIC"},  // Count: 2, Ordinal: 2146
            {"N'@ADDBD14 = '", "CIC"},  // Count: 2, Ordinal: 2147
            {"N'@ADDBD14 = NULL'", "DIC"},  // Count: 2, Ordinal: 2148
            {"N'@ADDBD15 = '", "EIC"},  // Count: 2, Ordinal: 2149
            {"N'@ADDBD15 = NULL'", "FIC"},  // Count: 2, Ordinal: 2150
            {"N'@ADDBD16 = '", "GIC"},  // Count: 2, Ordinal: 2151
            {"N'@ADDBD16 = NULL'", "HIC"},  // Count: 2, Ordinal: 2152
            {"N'@ADDBD17 = '", "IIC"},  // Count: 2, Ordinal: 2153
            {"N'@ADDBD17 = NULL'", "JIC"},  // Count: 2, Ordinal: 2154
            {"N'@ADDBD18 = '", "KIC"},  // Count: 2, Ordinal: 2155
            {"N'@ADDBD18 = NULL'", "LIC"},  // Count: 2, Ordinal: 2156
            {"N'@ADDBD19 = '", "MIC"},  // Count: 2, Ordinal: 2157
            {"N'@ADDBD19 = NULL'", "AJC"},  // Count: 2, Ordinal: 2158
            {"N'@ADDBD20 = '", "BJC"},  // Count: 2, Ordinal: 2159
            {"N'@ADDBD20 = NULL'", "CJC"},  // Count: 2, Ordinal: 2160
            {"N'@ADDBD21 = '", "DJC"},  // Count: 2, Ordinal: 2161
            {"N'@ADDBD21 = NULL'", "EJC"},  // Count: 2, Ordinal: 2162
            {"N'@ADDBD22 = '", "FJC"},  // Count: 2, Ordinal: 2163
            {"N'@ADDBD22 = NULL'", "GJC"},  // Count: 2, Ordinal: 2164
            {"N'@ADDBD23 = '", "HJC"},  // Count: 2, Ordinal: 2165
            {"N'@ADDBD23 = NULL'", "IJC"},  // Count: 2, Ordinal: 2166
            {"N'@ADDBD24 = '", "JJC"},  // Count: 2, Ordinal: 2167
            {"N'@ADDBD24 = NULL'", "KJC"},  // Count: 2, Ordinal: 2168
            {"N'@ADDBD25 = '", "LJC"},  // Count: 2, Ordinal: 2169
            {"N'@ADDBD25 = NULL'", "MJC"},  // Count: 2, Ordinal: 2170
            {"N'@ADDBD26 = '", "AKC"},  // Count: 2, Ordinal: 2171
            {"N'@ADDBD26 = NULL'", "BKC"},  // Count: 2, Ordinal: 2172
            {"N'@ADDBD27 = '", "CKC"},  // Count: 2, Ordinal: 2173
            {"N'@ADDBD27 = NULL'", "DKC"},  // Count: 2, Ordinal: 2174
            {"N'@ADDBD28 = '", "EKC"},  // Count: 2, Ordinal: 2175
            {"N'@ADDBD28 = NULL'", "FKC"},  // Count: 2, Ordinal: 2176
            {"N'@ADDBD29 = '", "GKC"},  // Count: 2, Ordinal: 2177
            {"N'@ADDBD29 = NULL'", "HKC"},  // Count: 2, Ordinal: 2178
            {"N'@ADDBD30 = '", "IKC"},  // Count: 2, Ordinal: 2179
            {"N'@ADDBD30 = NULL'", "JKC"},  // Count: 2, Ordinal: 2180
            {"N'@AMOUNT_IN_LOCAL_CURRENCY = '", "KKC"},  // Count: 2, Ordinal: 2181
            {"N'@AMOUNT_IN_LOCAL_CURRENCY = NULL'", "LKC"},  // Count: 2, Ordinal: 2182
            {"N'@BATCH_ID = '", "MKC"},  // Count: 2, Ordinal: 2183
            {"N'@BATCH_ID = NULL'", "ALC"},  // Count: 2, Ordinal: 2184
            {"N'@BATCH_PROD = '", "BLC"},  // Count: 2, Ordinal: 2185
            {"N'@BATCH_PROD = NULL'", "CLC"},  // Count: 2, Ordinal: 2186
            {"N'@CONTROL_RECIPE = '", "DLC"},  // Count: 2, Ordinal: 2187
            {"N'@CONTROL_RECIPE = NULL'", "ELC"},  // Count: 2, Ordinal: 2188
            {"N'@CONTROL_RECIPE_STATUS = '", "FLC"},  // Count: 2, Ordinal: 2189
            {"N'@CONTROL_RECIPE_STATUS = NULL'", "GLC"},  // Count: 2, Ordinal: 2190
            {"N'@DELBATCHES = '", "HLC"},  // Count: 2, Ordinal: 2191
            {"N'@DELBATCHES = NULL'", "ILC"},  // Count: 2, Ordinal: 2192
            {"N'@DELBD01 = '", "JLC"},  // Count: 2, Ordinal: 2193
            {"N'@DELBD01 = NULL'", "KLC"},  // Count: 2, Ordinal: 2194
            {"N'@DELBD02 = '", "LLC"},  // Count: 2, Ordinal: 2195
            {"N'@DELBD02 = NULL'", "MLC"},  // Count: 2, Ordinal: 2196
            {"N'@DELBD03 = '", "AMC"},  // Count: 2, Ordinal: 2197
            {"N'@DELBD03 = NULL'", "BMC"},  // Count: 2, Ordinal: 2198
            {"N'@DELBD04 = '", "CMC"},  // Count: 2, Ordinal: 2199
            {"N'@DELBD04 = NULL'", "DMC"},  // Count: 2, Ordinal: 2200
            {"N'@DELBD05 = '", "EMC"},  // Count: 2, Ordinal: 2201
            {"N'@DELBD05 = NULL'", "FMC"},  // Count: 2, Ordinal: 2202
            {"N'@DELBD06 = '", "GMC"},  // Count: 2, Ordinal: 2203
            {"N'@DELBD06 = NULL'", "HMC"},  // Count: 2, Ordinal: 2204
            {"N'@DELBD07 = '", "IMC"},  // Count: 2, Ordinal: 2205
            {"N'@DELBD07 = NULL'", "JMC"},  // Count: 2, Ordinal: 2206
            {"N'@DELBD08 = '", "KMC"},  // Count: 2, Ordinal: 2207
            {"N'@DELBD08 = NULL'", "LMC"},  // Count: 2, Ordinal: 2208
            {"N'@DELBD09 = '", "MMC"},  // Count: 2, Ordinal: 2209
            {"N'@DELBD09 = NULL'", "ANC"},  // Count: 2, Ordinal: 2210
            {"N'@DELBD10 = '", "BNC"},  // Count: 2, Ordinal: 2211
            {"N'@DELBD10 = NULL'", "CNC"},  // Count: 2, Ordinal: 2212
            {"N'@DELBD11 = '", "DNC"},  // Count: 2, Ordinal: 2213
            {"N'@DELBD11 = NULL'", "ENC"},  // Count: 2, Ordinal: 2214
            {"N'@DELBD12 = '", "FNC"},  // Count: 2, Ordinal: 2215
            {"N'@DELBD12 = NULL'", "GNC"},  // Count: 2, Ordinal: 2216
            {"N'@DELBD13 = '", "HNC"},  // Count: 2, Ordinal: 2217
            {"N'@DELBD13 = NULL'", "INC"},  // Count: 2, Ordinal: 2218
            {"N'@DELBD14 = '", "JNC"},  // Count: 2, Ordinal: 2219
            {"N'@DELBD14 = NULL'", "KNC"},  // Count: 2, Ordinal: 2220
            {"N'@DELBD15 = '", "LNC"},  // Count: 2, Ordinal: 2221
            {"N'@DELBD15 = NULL'", "MNC"},  // Count: 2, Ordinal: 2222
            {"N'@DELBD16 = '", "AOC"},  // Count: 2, Ordinal: 2223
            {"N'@DELBD16 = NULL'", "BOC"},  // Count: 2, Ordinal: 2224
            {"N'@DELBD17 = '", "COC"},  // Count: 2, Ordinal: 2225
            {"N'@DELBD17 = NULL'", "DOC"},  // Count: 2, Ordinal: 2226
            {"N'@DELBD18 = '", "EOC"},  // Count: 2, Ordinal: 2227
            {"N'@DELBD18 = NULL'", "FOC"},  // Count: 2, Ordinal: 2228
            {"N'@DELBD19 = '", "GOC"},  // Count: 2, Ordinal: 2229
            {"N'@DELBD19 = NULL'", "HOC"},  // Count: 2, Ordinal: 2230
            {"N'@DELBD20 = '", "IOC"},  // Count: 2, Ordinal: 2231
            {"N'@DELBD20 = NULL'", "JOC"},  // Count: 2, Ordinal: 2232
            {"N'@DELBD21 = '", "KOC"},  // Count: 2, Ordinal: 2233
            {"N'@DELBD21 = NULL'", "LOC"},  // Count: 2, Ordinal: 2234
            {"N'@DELBD22 = '", "MOC"},  // Count: 2, Ordinal: 2235
            {"N'@DELBD22 = NULL'", "APC"},  // Count: 2, Ordinal: 2236
            {"N'@DELBD23 = '", "BPC"},  // Count: 2, Ordinal: 2237
            {"N'@DELBD23 = NULL'", "CPC"},  // Count: 2, Ordinal: 2238
            {"N'@DELBD24 = '", "DPC"},  // Count: 2, Ordinal: 2239
            {"N'@DELBD24 = NULL'", "EPC"},  // Count: 2, Ordinal: 2240
            {"N'@DELBD25 = '", "FPC"},  // Count: 2, Ordinal: 2241
            {"N'@DELBD25 = NULL'", "GPC"},  // Count: 2, Ordinal: 2242
            {"N'@DELBD26 = '", "HPC"},  // Count: 2, Ordinal: 2243
            {"N'@DELBD26 = NULL'", "IPC"},  // Count: 2, Ordinal: 2244
            {"N'@DELBD27 = '", "JPC"},  // Count: 2, Ordinal: 2245
            {"N'@DELBD27 = NULL'", "KPC"},  // Count: 2, Ordinal: 2246
            {"N'@DELBD28 = '", "LPC"},  // Count: 2, Ordinal: 2247
            {"N'@DELBD28 = NULL'", "MPC"},  // Count: 2, Ordinal: 2248
            {"N'@DELBD29 = '", "AQC"},  // Count: 2, Ordinal: 2249
            {"N'@DELBD29 = NULL'", "BQC"},  // Count: 2, Ordinal: 2250
            {"N'@DELBD30 = '", "CQC"},  // Count: 2, Ordinal: 2251
            {"N'@DELBD30 = NULL'", "DQC"},  // Count: 2, Ordinal: 2252
            {"N'@DELIVERY_NOTE = '", "EQC"},  // Count: 2, Ordinal: 2253
            {"N'@DELIVERY_NOTE = NULL'", "FQC"},  // Count: 2, Ordinal: 2254
            {"N'@DELV01 = '", "GQC"},  // Count: 2, Ordinal: 2255
            {"N'@DELV01 = NULL'", "HQC"},  // Count: 2, Ordinal: 2256
            {"N'@DELV02 = NULL'", "IQC"},  // Count: 2, Ordinal: 2257
            {"N'@DELV03 = '", "JQC"},  // Count: 2, Ordinal: 2258
            {"N'@DELV03 = NULL'", "KQC"},  // Count: 2, Ordinal: 2259
            {"N'@DELV04 = '", "LQC"},  // Count: 2, Ordinal: 2260
            {"N'@DELV04 = NULL'", "MQC"},  // Count: 2, Ordinal: 2261
            {"N'@DELV05 = '", "ARC"},  // Count: 2, Ordinal: 2262
            {"N'@DELV05 = NULL'", "BRC"},  // Count: 2, Ordinal: 2263
            {"N'@DELV06 = '", "CRC"},  // Count: 2, Ordinal: 2264
            {"N'@DELV06 = NULL'", "DRC"},  // Count: 2, Ordinal: 2265
            {"N'@DELV07 = '", "ERC"},  // Count: 2, Ordinal: 2266
            {"N'@DELV07 = NULL'", "FRC"},  // Count: 2, Ordinal: 2267
            {"N'@DELV08 = '", "GRC"},  // Count: 2, Ordinal: 2268
            {"N'@DELV08 = NULL'", "HRC"},  // Count: 2, Ordinal: 2269
            {"N'@DELV09 = '", "IRC"},  // Count: 2, Ordinal: 2270
            {"N'@DELV09 = NULL'", "JRC"},  // Count: 2, Ordinal: 2271
            {"N'@DELV10 = '", "KRC"},  // Count: 2, Ordinal: 2272
            {"N'@DELV10 = NULL'", "LRC"},  // Count: 2, Ordinal: 2273
            {"N'@DELV11 = '", "MRC"},  // Count: 2, Ordinal: 2274
            {"N'@DELV11 = NULL'", "ASC"},  // Count: 2, Ordinal: 2275
            {"N'@DELV12 = '", "BSC"},  // Count: 2, Ordinal: 2276
            {"N'@DELV12 = NULL'", "CSC"},  // Count: 2, Ordinal: 2277
            {"N'@DELV13 = '", "DSC"},  // Count: 2, Ordinal: 2278
            {"N'@DELV13 = NULL'", "ESC"},  // Count: 2, Ordinal: 2279
            {"N'@DELV14 = '", "FSC"},  // Count: 2, Ordinal: 2280
            {"N'@DELV14 = NULL'", "GSC"},  // Count: 2, Ordinal: 2281
            {"N'@DELV15 = '", "HSC"},  // Count: 2, Ordinal: 2282
            {"N'@DELV15 = NULL'", "ISC"},  // Count: 2, Ordinal: 2283
            {"N'@DELV16 = '", "JSC"},  // Count: 2, Ordinal: 2284
            {"N'@DELV16 = NULL'", "KSC"},  // Count: 2, Ordinal: 2285
            {"N'@DELV17 = '", "LSC"},  // Count: 2, Ordinal: 2286
            {"N'@DELV17 = NULL'", "MSC"},  // Count: 2, Ordinal: 2287
            {"N'@DELV18 = '", "ATC"},  // Count: 2, Ordinal: 2288
            {"N'@DELV18 = NULL'", "BTC"},  // Count: 2, Ordinal: 2289
            {"N'@DELV19 = '", "CTC"},  // Count: 2, Ordinal: 2290
            {"N'@DELV19 = NULL'", "DTC"},  // Count: 2, Ordinal: 2291
            {"N'@DELV20 = '", "ETC"},  // Count: 2, Ordinal: 2292
            {"N'@DELV20 = NULL'", "FTC"},  // Count: 2, Ordinal: 2293
            {"N'@DELV21 = '", "GTC"},  // Count: 2, Ordinal: 2294
            {"N'@DELV21 = NULL'", "HTC"},  // Count: 2, Ordinal: 2295
            {"N'@DELV22 = '", "ITC"},  // Count: 2, Ordinal: 2296
            {"N'@DELV22 = NULL'", "JTC"},  // Count: 2, Ordinal: 2297
            {"N'@DELV23 = '", "KTC"},  // Count: 2, Ordinal: 2298
            {"N'@DELV23 = NULL'", "LTC"},  // Count: 2, Ordinal: 2299
            {"N'@DELV24 = '", "MTC"},  // Count: 2, Ordinal: 2300
            {"N'@DELV24 = NULL'", "AUC"},  // Count: 2, Ordinal: 2301
            {"N'@DELV25 = '", "BUC"},  // Count: 2, Ordinal: 2302
            {"N'@DELV25 = NULL'", "CUC"},  // Count: 2, Ordinal: 2303
            {"N'@DELV26 = '", "DUC"},  // Count: 2, Ordinal: 2304
            {"N'@DELV26 = NULL'", "EUC"},  // Count: 2, Ordinal: 2305
            {"N'@DELV27 = '", "FUC"},  // Count: 2, Ordinal: 2306
            {"N'@DELV27 = NULL'", "GUC"},  // Count: 2, Ordinal: 2307
            {"N'@DELV28 = '", "HUC"},  // Count: 2, Ordinal: 2308
            {"N'@DELV28 = NULL'", "IUC"},  // Count: 2, Ordinal: 2309
            {"N'@DELV29 = '", "JUC"},  // Count: 2, Ordinal: 2310
            {"N'@DELV29 = NULL'", "KUC"},  // Count: 2, Ordinal: 2311
            {"N'@DELV30 = '", "LUC"},  // Count: 2, Ordinal: 2312
            {"N'@DELV30 = NULL'", "MUC"},  // Count: 2, Ordinal: 2313
            {"N'@GIFLAG = '", "AVC"},  // Count: 2, Ordinal: 2314
            {"N'@GIFLAG = NULL'", "BVC"},  // Count: 2, Ordinal: 2315
            {"N'@INVENTORY_QUANTITY = '", "CVC"},  // Count: 2, Ordinal: 2316
            {"N'@INVENTORY_QUANTITY = NULL'", "DVC"},  // Count: 2, Ordinal: 2317
            {"N'@MEANSTRANSPORT = '", "EVC"},  // Count: 2, Ordinal: 2318
            {"N'@MEANSTRANSPORT = NULL'", "FVC"},  // Count: 2, Ordinal: 2319
            {"N'@NAME1 = '", "GVC"},  // Count: 2, Ordinal: 2320
            {"N'@NAME1 = NULL'", "HVC"},  // Count: 2, Ordinal: 2321
            {"N'@NAME10 = '", "IVC"},  // Count: 2, Ordinal: 2322
            {"N'@NAME10 = NULL'", "JVC"},  // Count: 2, Ordinal: 2323
            {"N'@NAME11 = '", "KVC"},  // Count: 2, Ordinal: 2324
            {"N'@NAME11 = NULL'", "LVC"},  // Count: 2, Ordinal: 2325
            {"N'@NAME12 = '", "MVC"},  // Count: 2, Ordinal: 2326
            {"N'@NAME12 = NULL'", "AWC"},  // Count: 2, Ordinal: 2327
            {"N'@NAME13 = '", "BWC"},  // Count: 2, Ordinal: 2328
            {"N'@NAME13 = NULL'", "CWC"},  // Count: 2, Ordinal: 2329
            {"N'@NAME14 = '", "DWC"},  // Count: 2, Ordinal: 2330
            {"N'@NAME14 = NULL'", "EWC"},  // Count: 2, Ordinal: 2331
            {"N'@NAME15 = '", "FWC"},  // Count: 2, Ordinal: 2332
            {"N'@NAME15 = NULL'", "GWC"},  // Count: 2, Ordinal: 2333
            {"N'@NAME16 = '", "HWC"},  // Count: 2, Ordinal: 2334
            {"N'@NAME16 = NULL'", "IWC"},  // Count: 2, Ordinal: 2335
            {"N'@NAME17 = '", "JWC"},  // Count: 2, Ordinal: 2336
            {"N'@NAME17 = NULL'", "KWC"},  // Count: 2, Ordinal: 2337
            {"N'@NAME18 = '", "LWC"},  // Count: 2, Ordinal: 2338
            {"N'@NAME18 = NULL'", "MWC"},  // Count: 2, Ordinal: 2339
            {"N'@NAME19 = '", "AXC"},  // Count: 2, Ordinal: 2340
            {"N'@NAME19 = NULL'", "BXC"},  // Count: 2, Ordinal: 2341
            {"N'@NAME2 = '", "CXC"},  // Count: 2, Ordinal: 2342
            {"N'@NAME2 = NULL'", "DXC"},  // Count: 2, Ordinal: 2343
            {"N'@NAME20 = '", "EXC"},  // Count: 2, Ordinal: 2344
            {"N'@NAME20 = NULL'", "FXC"},  // Count: 2, Ordinal: 2345
            {"N'@NAME3 = '", "GXC"},  // Count: 2, Ordinal: 2346
            {"N'@NAME3 = NULL'", "HXC"},  // Count: 2, Ordinal: 2347
            {"N'@NAME4 = '", "IXC"},  // Count: 2, Ordinal: 2348
            {"N'@NAME4 = NULL'", "JXC"},  // Count: 2, Ordinal: 2349
            {"N'@NAME5 = '", "KXC"},  // Count: 2, Ordinal: 2350
            {"N'@NAME5 = NULL'", "LXC"},  // Count: 2, Ordinal: 2351
            {"N'@NAME6 = '", "MXC"},  // Count: 2, Ordinal: 2352
            {"N'@NAME6 = NULL'", "AYC"},  // Count: 2, Ordinal: 2353
            {"N'@NAME7 = '", "BYC"},  // Count: 2, Ordinal: 2354
            {"N'@NAME7 = NULL'", "CYC"},  // Count: 2, Ordinal: 2355
            {"N'@NAME8 = '", "DYC"},  // Count: 2, Ordinal: 2356
            {"N'@NAME8 = NULL'", "EYC"},  // Count: 2, Ordinal: 2357
            {"N'@NAME9 = '", "FYC"},  // Count: 2, Ordinal: 2358
            {"N'@NAME9 = NULL'", "GYC"},  // Count: 2, Ordinal: 2359
            {"N'@PLANT = '", "HYC"},  // Count: 2, Ordinal: 2360
            {"N'@PLANT = NULL'", "IYC"},  // Count: 2, Ordinal: 2361
            {"N'@POSTINGDATE = '", "JYC"},  // Count: 2, Ordinal: 2362
            {"N'@POSTINGDATE = NULL'", "KYC"},  // Count: 2, Ordinal: 2363
            {"N'@REJ_MATERIAL = '", "LYC"},  // Count: 2, Ordinal: 2364
            {"N'@REJ_MATERIAL = NULL'", "MYC"},  // Count: 2, Ordinal: 2365
            {"N'@REJ_PROCESS_ORDER = '", "AZC"},  // Count: 2, Ordinal: 2366
            {"N'@REJ_PROCESS_ORDER = NULL'", "BZC"},  // Count: 2, Ordinal: 2367
            {"N'@REJ_QUANTITY = '", "CZC"},  // Count: 2, Ordinal: 2368
            {"N'@REJ_QUANTITY = NULL'", "DZC"},  // Count: 2, Ordinal: 2369
            {"N'@SEALS = '", "EZC"},  // Count: 2, Ordinal: 2370
            {"N'@SEALS = NULL'", "FZC"},  // Count: 2, Ordinal: 2371
            {"N'@SHIP_TO = '", "GZC"},  // Count: 2, Ordinal: 2372
            {"N'@SHIP_TO = NULL'", "HZC"},  // Count: 2, Ordinal: 2373
            {"N'@SHIPPOINT = '", "IZC"},  // Count: 2, Ordinal: 2374
            {"N'@SHIPPOINT = NULL'", "JZC"},  // Count: 2, Ordinal: 2375
            {"N'@STATUS_CONF = '", "KZC"},  // Count: 2, Ordinal: 2376
            {"N'@STATUS_CONF = NULL'", "LZC"},  // Count: 2, Ordinal: 2377
            {"N'@TRANSPORTID = '", "MZC"},  // Count: 2, Ordinal: 2378
            {"N'@TRANSPORTID = NULL'", "AaC"},  // Count: 2, Ordinal: 2379
            {"N'@UNIT = '", "BaC"},  // Count: 2, Ordinal: 2380
            {"N'@UNIT = NULL'", "CaC"},  // Count: 2, Ordinal: 2381
            {"N'@VALUE1 = '", "DaC"},  // Count: 2, Ordinal: 2382
            {"N'@VALUE1 = NULL'", "EaC"},  // Count: 2, Ordinal: 2383
            {"N'@VALUE11 = '", "FaC"},  // Count: 2, Ordinal: 2384
            {"N'@VALUE11 = NULL'", "GaC"},  // Count: 2, Ordinal: 2385
            {"N'@VALUE12 = '", "HaC"},  // Count: 2, Ordinal: 2386
            {"N'@VALUE12 = NULL'", "IaC"},  // Count: 2, Ordinal: 2387
            {"N'@VALUE13 = '", "JaC"},  // Count: 2, Ordinal: 2388
            {"N'@VALUE13 = NULL'", "KaC"},  // Count: 2, Ordinal: 2389
            {"N'@VALUE14 = '", "LaC"},  // Count: 2, Ordinal: 2390
            {"N'@VALUE14 = NULL'", "MaC"},  // Count: 2, Ordinal: 2391
            {"N'@VALUE15 = '", "AbC"},  // Count: 2, Ordinal: 2392
            {"N'@VALUE15 = NULL'", "BbC"},  // Count: 2, Ordinal: 2393
            {"N'@VALUE16 = '", "CbC"},  // Count: 2, Ordinal: 2394
            {"N'@VALUE16 = NULL'", "DbC"},  // Count: 2, Ordinal: 2395
            {"N'@VALUE17 = '", "EbC"},  // Count: 2, Ordinal: 2396
            {"N'@VALUE17 = NULL'", "FbC"},  // Count: 2, Ordinal: 2397
            {"N'@VALUE18 = '", "GbC"},  // Count: 2, Ordinal: 2398
            {"N'@VALUE18 = NULL'", "HbC"},  // Count: 2, Ordinal: 2399
            {"N'@VALUE19 = '", "IbC"},  // Count: 2, Ordinal: 2400
            {"N'@VALUE19 = NULL'", "JbC"},  // Count: 2, Ordinal: 2401
            {"N'@VALUE2 = '", "KbC"},  // Count: 2, Ordinal: 2402
            {"N'@VALUE2 = NULL'", "LbC"},  // Count: 2, Ordinal: 2403
            {"N'@VALUE3 = '", "MbC"},  // Count: 2, Ordinal: 2404
            {"N'@VALUE3 = NULL'", "AcC"},  // Count: 2, Ordinal: 2405
            {"N'@VALUE4 = '", "BcC"},  // Count: 2, Ordinal: 2406
            {"N'@VALUE4 = NULL'", "CcC"},  // Count: 2, Ordinal: 2407
            {"N'@VALUE5 = '", "DcC"},  // Count: 2, Ordinal: 2408
            {"N'@VALUE5 = NULL'", "EcC"},  // Count: 2, Ordinal: 2409
            {"N'@VALUE6 = '", "FcC"},  // Count: 2, Ordinal: 2410
            {"N'@VALUE6 = NULL'", "GcC"},  // Count: 2, Ordinal: 2411
            {"N'@VALUE7 = '", "HcC"},  // Count: 2, Ordinal: 2412
            {"N'@VALUE7 = NULL'", "IcC"},  // Count: 2, Ordinal: 2413
            {"N'@VALUE8 = '", "JcC"},  // Count: 2, Ordinal: 2414
            {"N'@VALUE8 = NULL'", "KcC"},  // Count: 2, Ordinal: 2415
            {"N'@VALUE9 = '", "LcC"},  // Count: 2, Ordinal: 2416
            {"N'@VALUE9 = NULL'", "McC"},  // Count: 2, Ordinal: 2417
            {"N'|'", "AdC"},  // Count: 2, Ordinal: 2418
            {"N'||'", "BdC"},  // Count: 2, Ordinal: 2419
            {"N'ALL'", "CdC"},  // Count: 2, Ordinal: 2420
            {"N'D'", "DdC"},  // Count: 2, Ordinal: 2421
            {"N'DATABASENAME'", "EdC"},  // Count: 2, Ordinal: 2422
            {"N'EMN_SP_SEND_YDELV'", "FdC"},  // Count: 2, Ordinal: 2423
            {"NEWLCL", "GdC"},  // Count: 2, Ordinal: 2424
            {"NEWTARG", "HdC"},  // Count: 2, Ordinal: 2425
            {"NEWUCL", "IdC"},  // Count: 2, Ordinal: 2426
            {"N'EXCLUDEPROGRAMNAME'", "JdC"},  // Count: 2, Ordinal: 2427
            {"N'I'", "KdC"},  // Count: 2, Ordinal: 2428
            {"N'INCLUDEPROGRAMNAME'", "LdC"},  // Count: 2, Ordinal: 2429
            {"N'LIST'", "MdC"},  // Count: 2, Ordinal: 2430
            {"N'MUST SPECIFY A VALID RULEID'", "AeC"},  // Count: 2, Ordinal: 2431
            {"N'MUST SPECIFY A VALID STRATEGYID'", "BeC"},  // Count: 2, Ordinal: 2432
            {"N'NONE'", "CeC"},  // Count: 2, Ordinal: 2433
            {"N'PI_CRST'", "DeC"},  // Count: 2, Ordinal: 2434
            {"N'SELECT CAUSEID FROM CSSTRATEGYCAUSES WHERE STRATEGYID = '", "EeC"},  // Count: 2, Ordinal: 2435
            {"N'SELECT DISTINCT DESCRIPTION ", "FeC"},  // Count: 2, Ordinal: 2436
            {"N'STARTING . . . '", "GeC"},  // Count: 2, Ordinal: 2437
            {"N'YDELV'", "HeC"},  // Count: 2, Ordinal: 2438
            {"N'YINVADJ'", "IeC"},  // Count: 2, Ordinal: 2439
            {"N'YXFER'", "JeC"},  // Count: 2, Ordinal: 2440
            {"ON_ERROR2:", "KeC"},  // Count: 2, Ordinal: 2441
            {"ON_ERROR3:", "LeC"},  // Count: 2, Ordinal: 2442
            {"ON_ERROR4:", "MeC"},  // Count: 2, Ordinal: 2443
            {"ORDER_LIST_DESCRIPTION", "AfC"},  // Count: 2, Ordinal: 2444
            {"PCBO_AREA", "BfC"},  // Count: 2, Ordinal: 2445
            {"PCBO_BLEND_TRACKING", "CfC"},  // Count: 2, Ordinal: 2446
            {"PCBO_DETAILED_PRODUCTION_TRACKING", "DfC"},  // Count: 2, Ordinal: 2447
            {"PCBO_EST_PROD_TRACKING", "EfC"},  // Count: 2, Ordinal: 2448
            {"PCBO_LABOR_EQUIP_TRACKING", "FfC"},  // Count: 2, Ordinal: 2449
            {"PCNAME", "GfC"},  // Count: 2, Ordinal: 2450
            {"PCOPERATINGSYSTEM", "HfC"},  // Count: 2, Ordinal: 2451
            {"PI_DESCRIPTION", "IfC"},  // Count: 2, Ordinal: 2452
            {"PI_NODE_NAME", "JfC"},  // Count: 2, Ordinal: 2453
            {"PIMSAREA", "KfC"},  // Count: 2, Ordinal: 2454
            {"PIMSCLIENTVERSION", "LfC"},  // Count: 2, Ordinal: 2455
            {"PIMSENVIRONMENTCHANGEHISTORY", "MfC"},  // Count: 2, Ordinal: 2456
            {"PIMSPENDINGRLINKTRANSACTIONS'", "AgC"},  // Count: 2, Ordinal: 2457
            {"PIMSPENDINGRLINKTRANSACTIONS_BAK", "BgC"},  // Count: 2, Ordinal: 2458
            {"PIMSPISERVER", "CgC"},  // Count: 2, Ordinal: 2459
            {"PIMSPIUSERID", "DgC"},  // Count: 2, Ordinal: 2460
            {"PIMSSDKVERSION", "EgC"},  // Count: 2, Ordinal: 2461
            {"PIMSSTORAGELOCATION", "FgC"},  // Count: 2, Ordinal: 2462
            {"PIMSURL", "GgC"},  // Count: 2, Ordinal: 2463
            {"PISERVERNAME", "HgC"},  // Count: 2, Ordinal: 2464
            {"PITAGDESC", "IgC"},  // Count: 2, Ordinal: 2465
            {"PLANT_ID", "JgC"},  // Count: 2, Ordinal: 2466
            {"POSTDATETIME", "KgC"},  // Count: 2, Ordinal: 2467
            {"PROCESS_MESSAGE", "LgC"},  // Count: 2, Ordinal: 2468
            {"PROCESS_ORDER_NUMBER", "MgC"},  // Count: 2, Ordinal: 2469
            {"PRODUCT_GMN", "AhC"},  // Count: 2, Ordinal: 2470
            {"QUANTITYTYPE", "BhC"},  // Count: 2, Ordinal: 2471
            {"RAW_BATCH_ID", "ChC"},  // Count: 2, Ordinal: 2472
            {"RECIPE", "DhC"},  // Count: 2, Ordinal: 2473
            {"REPORTLINK", "EhC"},  // Count: 2, Ordinal: 2474
            {"RULEID'", "FhC"},  // Count: 2, Ordinal: 2475
            {"SAMPLE_DATE", "GhC"},  // Count: 2, Ordinal: 2476
            {"SECPIMSCHANGEHISTORY", "HhC"},  // Count: 2, Ordinal: 2477
            {"SERVERTIME", "IhC"},  // Count: 2, Ordinal: 2478
            {"'SET %'", "JhC"},  // Count: 2, Ordinal: 2479
            {"SHIFT", "KhC"},  // Count: 2, Ordinal: 2480
            {"SP_DELETE_LISTANDTAGS", "LhC"},  // Count: 2, Ordinal: 2481
            {"SP_DELETE_LISTAREALISTSANDTAGS", "MhC"},  // Count: 2, Ordinal: 2482
            {"SP_DELETE_PRODUCTION_UNIT_1", "AiC"},  // Count: 2, Ordinal: 2483
            {"SP_INSERT_TAGS_1", "BiC"},  // Count: 2, Ordinal: 2484
            {"SP_INSERTKEYLISTPOINTS", "CiC"},  // Count: 2, Ordinal: 2485
            {"SP_UPDATEKEYLIST", "DiC"},  // Count: 2, Ordinal: 2486
            {"SP_UPDATEKEYLISTAREA", "EiC"},  // Count: 2, Ordinal: 2487
            {"SP_UPDATEKEYLISTPOINTS", "FiC"},  // Count: 2, Ordinal: 2488
            {"SQLUSERID", "GiC"},  // Count: 2, Ordinal: 2489
            {"START_TRACE", "HiC"},  // Count: 2, Ordinal: 2490
            {"STOCKTYPE", "IiC"},  // Count: 2, Ordinal: 2491
            {"SUPPLEMENTAL", "JiC"},  // Count: 2, Ordinal: 2492
            {"SUSER_NAME", "KiC"},  // Count: 2, Ordinal: 2493
            {"TAGSWEREUPDATED", "LiC"},  // Count: 2, Ordinal: 2494
            {"TEMPXML", "MiC"},  // Count: 2, Ordinal: 2495
            {"USP_CS_DELETE_CSCRITERIA_BY_RULEID", "AjC"},  // Count: 2, Ordinal: 2496
            {"USP_CS_DELETE_CSSTRATEGYTAGS_BY_STRATEGYID", "BjC"},  // Count: 2, Ordinal: 2497
            {"USP_CS_INSERT_CSCAUSERELATIONS", "CjC"},  // Count: 2, Ordinal: 2498
            {"USP_CS_INSERT_CSCAUSERELATIONS1", "DjC"},  // Count: 2, Ordinal: 2499
            {"USP_CS_INSERT_CSSTRATEGIES", "EjC"},  // Count: 2, Ordinal: 2500
            {"USP_CS_INSERT_CSSTRATEGYCAUSES", "FjC"},  // Count: 2, Ordinal: 2501
            {"USP_CS_INSERT_CSSTRATEGYCAUSES1", "GjC"},  // Count: 2, Ordinal: 2502
            {"USP_CS_INSERT_CSSTRATEGYTAGS", "HjC"},  // Count: 2, Ordinal: 2503
            {"VALIDTAGVALUE", "IjC"},  // Count: 2, Ordinal: 2504
            {"VER", "JjC"},  // Count: 2, Ordinal: 2505
            {"WINUSERID", "KjC"},  // Count: 2, Ordinal: 2506
            {"XP_CMDSHELL", "LjC"},  // Count: 2, Ordinal: 2507
            {"YINVADJ_MESSAGELOG", "MjC"},  // Count: 2, Ordinal: 2508
            {"YINVADJ_SUMMARIZEDPIINVENTORY", "AkC"},  // Count: 2, Ordinal: 2509
            {"-60", "BkC"},  // Count: 1, Ordinal: 2510
            {"-3", "CkC"},  // Count: 1, Ordinal: 2511
            {"3048", "GkC"},  // Count: 1, Ordinal: 2515
            {"'*'", "JkC"},  // Count: 1, Ordinal: 2518
            {"::FN_TRACE_GETTABLE", "KkC"},  // Count: 1, Ordinal: 2519
            {"@@ERROR<>", "LkC"},  // Count: 1, Ordinal: 2520
            {"@@SERVERNAME", "MkC"},  // Count: 1, Ordinal: 2521
            {"-@ARCHIVEDAYS", "AlC"},  // Count: 1, Ordinal: 2522
            {"'@BATCH '", "BlC"},  // Count: 1, Ordinal: 2523
            {"'@BATCH = NULL'", "ClC"},  // Count: 1, Ordinal: 2524
            {"@BATSW", "DlC"},  // Count: 1, Ordinal: 2525
            {"@DELSTAT", "ElC"},  // Count: 1, Ordinal: 2526
            {"'@EVENT_DATE '", "FlC"},  // Count: 1, Ordinal: 2527
            {"'@EVENT_DATE = NULL'", "GlC"},  // Count: 1, Ordinal: 2528
            {"'@EVENT_TIME '", "HlC"},  // Count: 1, Ordinal: 2529
            {"'@EVENT_TIME = NULL'", "IlC"},  // Count: 1, Ordinal: 2530
            {"'@INVENTORY_QUANTITY '", "JlC"},  // Count: 1, Ordinal: 2531
            {"'@INVENTORY_QUANTITY = NULL'", "KlC"},  // Count: 1, Ordinal: 2532
            {"'@MATERIAL '", "LlC"},  // Count: 1, Ordinal: 2533
            {"'@MATERIAL = NULL'", "MlC"},  // Count: 1, Ordinal: 2534
            {"'@MSCLA '", "AmC"},  // Count: 1, Ordinal: 2535
            {"'@MSCLA = NULL'", "BmC"},  // Count: 1, Ordinal: 2536
            {"@MSGCODE", "CmC"},  // Count: 1, Ordinal: 2537
            {"'@PLANT_ID '", "DmC"},  // Count: 1, Ordinal: 2538
            {"'@PLANT_ID = NULL'", "EmC"},  // Count: 1, Ordinal: 2539
            {"'@QUANTITY_TYPE '", "FmC"},  // Count: 1, Ordinal: 2540
            {"'@QUANTITY_TYPE = NULL'", "GmC"},  // Count: 1, Ordinal: 2541
            {"'@STOCK_TYPE '", "HmC"},  // Count: 1, Ordinal: 2542
            {"'@STOCK_TYPE = NULL'", "ImC"},  // Count: 1, Ordinal: 2543
            {"'@STORAGE_LOCATION '", "JmC"},  // Count: 1, Ordinal: 2544
            {"'@STORAGE_LOCATION = NULL'", "KmC"},  // Count: 1, Ordinal: 2545
            {"'@UNIT_OF_MEASURE '", "LmC"},  // Count: 1, Ordinal: 2546
            {"'@UNIT_OF_MEASURE = NULL'", "MmC"},  // Count: 1, Ordinal: 2547
            {"@X", "AnC"},  // Count: 1, Ordinal: 2548
            {"@Y", "BnC"},  // Count: 1, Ordinal: 2549
            {"<>''", "CnC"},  // Count: 1, Ordinal: 2550
            {">0", "DnC"},  // Count: 1, Ordinal: 2551
            {"'2'", "EnC"},  // Count: 1, Ordinal: 2552
            {"4'", "FnC"},  // Count: 1, Ordinal: 2553
            {"ACTIONID'", "GnC"},  // Count: 1, Ordinal: 2554
            {"ADMINUSERS", "HnC"},  // Count: 1, Ordinal: 2555
            {"'ALL'", "InC"},  // Count: 1, Ordinal: 2556
            {"ARCSHIPPEDBATCHES", "JnC"},  // Count: 1, Ordinal: 2557
            {"AVAILABLE", "KnC"},  // Count: 1, Ordinal: 2558
            {"BAL_BALES", "LnC"},  // Count: 1, Ordinal: 2559
            {"BALEID", "MnC"},  // Count: 1, Ordinal: 2560
            {"BALER_NUMBER", "AoC"},  // Count: 1, Ordinal: 2561
            {"BATCH_FORMAT", "BoC"},  // Count: 1, Ordinal: 2562
            {"BATCH_UOM", "CoC"},  // Count: 1, Ordinal: 2563
            {"BATCHID_TAG", "DoC"},  // Count: 1, Ordinal: 2564
            {"BETWEEN", "EoC"},  // Count: 1, Ordinal: 2565
            {"BLEND_BATCH_ID", "FoC"},  // Count: 1, Ordinal: 2566
            {"'CAN NOT START NEW TRACE'", "GoC"},  // Count: 1, Ordinal: 2567
            {"CAUSACT'", "HoC"},  // Count: 1, Ordinal: 2568
            {"CITATIONID'", "IoC"},  // Count: 1, Ordinal: 2569
            {"CONTAINERID", "JoC"},  // Count: 1, Ordinal: 2570
            {"COS_BALER", "KoC"},  // Count: 1, Ordinal: 2571
            {"COS_RETRIEVEBALERINFO", "LoC"},  // Count: 1, Ordinal: 2572
            {"CREW_NUMBER", "MoC"},  // Count: 1, Ordinal: 2573
            {"CS_ALLOWOTHERACTION", "ApC"},  // Count: 1, Ordinal: 2574
            {"CS_ALLOWOTHERCAUSE", "BpC"},  // Count: 1, Ordinal: 2575
            {"CS_ARCHIVEMAIL", "CpC"},  // Count: 1, Ordinal: 2576
            {"CS_CLEARCRITERIASTATUS", "DpC"},  // Count: 1, Ordinal: 2577
            {"CS_CLOSEACITATION", "EpC"},  // Count: 1, Ordinal: 2578
            {"CS_CLOSECITATIONENTRY", "FpC"},  // Count: 1, Ordinal: 2579
            {"CS_CREATEMAIL", "GpC"},  // Count: 1, Ordinal: 2580
            {"CS_CRITERIALIST", "HpC"},  // Count: 1, Ordinal: 2581
            {"CS_DEACTIVATERULE", "IpC"},  // Count: 1, Ordinal: 2582
            {"CS_DELETE_CSACTIONMASTER_1", "JpC"},  // Count: 1, Ordinal: 2583
            {"CS_DELETE_CSACTIONTAGS_BY_ACTIONID", "KpC"},  // Count: 1, Ordinal: 2584
            {"CS_DELETE_CSACTIONTAGS_BY_ACTIONID_TAGNAME", "LpC"},  // Count: 1, Ordinal: 2585
            {"CS_DELETE_CSACTIONTAGSLOG_BY_LASTCHANGEDATE", "MpC"},  // Count: 1, Ordinal: 2586
            {"CS_DELETE_CSAREAS_1", "AqC"},  // Count: 1, Ordinal: 2587
            {"CS_DELETE_CSCAUSEACTION_1", "BqC"},  // Count: 1, Ordinal: 2588
            {"CS_DELETE_CSCAUSERELATIONS_1", "CqC"},  // Count: 1, Ordinal: 2589
            {"CS_DELETE_CSCAUSES_1", "DqC"},  // Count: 1, Ordinal: 2590
            {"CS_DELETE_CSCAUSESANDRELATIONS_1", "EqC"},  // Count: 1, Ordinal: 2591
            {"CS_DELETE_CSDEADTIMELIST_BY_RESETTIME", "FqC"},  // Count: 1, Ordinal: 2592
            {"CS_DELETE_CSDEADTIMELIST_BY_STATUS", "GqC"},  // Count: 1, Ordinal: 2593
            {"CS_DELETE_CSDEADTIMELIST_BY_STATUS_DELAYTIME", "HqC"},  // Count: 1, Ordinal: 2594
            {"CS_DELETE_CSDEADTIMELIST_BY_STATUS_RESETTIME", "IqC"},  // Count: 1, Ordinal: 2595
            {"CS_DELETE_CSDEADTIMELIST_BY_TAGNAME", "JqC"},  // Count: 1, Ordinal: 2596
            {"CS_DELETE_CSDEADTIMELIST_BY_TAGNAME_DATECREATED", "KqC"},  // Count: 1, Ordinal: 2597
            {"CS_DELETE_CSDEADTIMELIST_BY_TAGNAME_DATECREATED_ACTIONID", "LqC"},  // Count: 1, Ordinal: 2598
            {"CS_DELETE_CSDEADTIMELISTLOG_BY_DATECREATED", "MqC"},  // Count: 1, Ordinal: 2599
            {"CS_DELETE_CSSTRATEGIES_1", "ArC"},  // Count: 1, Ordinal: 2600
            {"CS_DELETE_CSSTRATEGYCAUSES_1", "BrC"},  // Count: 1, Ordinal: 2601
            {"CS_DELETERULE", "CrC"},  // Count: 1, Ordinal: 2602
            {"CS_FORBIDOTHERACTION", "DrC"},  // Count: 1, Ordinal: 2603
            {"CS_FORBIDOTHERCAUSE", "ErC"},  // Count: 1, Ordinal: 2604
            {"CS_GETACAUSE", "FrC"},  // Count: 1, Ordinal: 2605
            {"CS_GETACTIONINSTRUCTION", "GrC"},  // Count: 1, Ordinal: 2606
            {"CS_GETACTIONSFORACAUSE", "HrC"},  // Count: 1, Ordinal: 2607
            {"CS_GETACTIVECRITERIA", "IrC"},  // Count: 1, Ordinal: 2608
            {"CS_GETACTIVERULES", "JrC"},  // Count: 1, Ordinal: 2609
            {"CS_GETACTIVERULES_HWD", "KrC"},  // Count: 1, Ordinal: 2610
            {"CS_GETALIASEDCRITERIA", "LrC"},  // Count: 1, Ordinal: 2611
            {"CS_GETALLAREAS", "MrC"},  // Count: 1, Ordinal: 2612
            {"CS_GETALLCITATIONHISTORYBYSTRAGEGYID", "AsC"},  // Count: 1, Ordinal: 2613
            {"CS_GETALLCITATIONHISTORYBYSTRATEGYID", "BsC"},  // Count: 1, Ordinal: 2614
            {"CS_GETALLRULES", "CsC"},  // Count: 1, Ordinal: 2615
            {"CS_GETALLSTRATEGIES", "DsC"},  // Count: 1, Ordinal: 2616
            {"CS_GETAREABYNAME", "EsC"},  // Count: 1, Ordinal: 2617
            {"CS_GETAREABYRULEID", "FsC"},  // Count: 1, Ordinal: 2618
            {"CS_GETARULEBYNAME", "GsC"},  // Count: 1, Ordinal: 2619
            {"CS_GETASTRATEGY", "HsC"},  // Count: 1, Ordinal: 2620
            {"CS_GETASTRATEGYBYNAME", "IsC"},  // Count: 1, Ordinal: 2621
            {"CS_GETASTRATEGYBYTAGNAME", "JsC"},  // Count: 1, Ordinal: 2622
            {"CS_GETCAUSEIDDESCBYPARENTCAUSEID", "KsC"},  // Count: 1, Ordinal: 2623
            {"CS_GETCAUSEIDDESCBYSTRATEGYID", "LsC"},  // Count: 1, Ordinal: 2624
            {"CS_GETCAUSEINSTRUCTION", "MsC"},  // Count: 1, Ordinal: 2625
            {"CS_GETCAUSERELATIONS", "AtC"},  // Count: 1, Ordinal: 2626
            {"CS_GETCITATIONHISTORYBYSTRAGEGYID", "BtC"},  // Count: 1, Ordinal: 2627
            {"CS_GETCITATIONLOG", "CtC"},  // Count: 1, Ordinal: 2628
            {"CS_GETCITATIONRESPONSEID", "DtC"},  // Count: 1, Ordinal: 2629
            {"CS_GETCITATIONS", "EtC"},  // Count: 1, Ordinal: 2630
            {"CS_GETCITATIONSFORARULE", "FtC"},  // Count: 1, Ordinal: 2631
            {"CS_GETCITATIONSINALARM", "GtC"},  // Count: 1, Ordinal: 2632
            {"CS_GETCITATIONSINALARM_WITH_CRITERIA", "HtC"},  // Count: 1, Ordinal: 2633
            {"CS_GETCITATIONSINALARM_WITH_CRITERIA_AND_FILTER", "ItC"},  // Count: 1, Ordinal: 2634
            {"CS_GETCITATIONSINALARM_WITH_CRITERIA_AND_FILTER_AND_RULES", "JtC"},  // Count: 1, Ordinal: 2635
            {"CS_GETCITATIONSINALARMFORSEARCH", "KtC"},  // Count: 1, Ordinal: 2636
            {"CS_GETCITATIONSINALARMFORSEARCH_WITH_CRITERIA", "LtC"},  // Count: 1, Ordinal: 2637
            {"CS_GETCITATIONSINALARMFORSEARCH_WITH_CRITERIA_FILTER", "MtC"},  // Count: 1, Ordinal: 2638
            {"CS_GETCLOSEDCITATIONSAFTERTIME", "AuC"},  // Count: 1, Ordinal: 2639
            {"CS_GETCRITERIAFORARULE", "BuC"},  // Count: 1, Ordinal: 2640
            {"CS_GETLISTOFAREAS", "CuC"},  // Count: 1, Ordinal: 2641
            {"CS_GETLISTOFRULESBYAREA", "DuC"},  // Count: 1, Ordinal: 2642
            {"CS_GETLISTOFSTRATEGIES", "EuC"},  // Count: 1, Ordinal: 2643
            {"CS_GETMAIL", "FuC"},  // Count: 1, Ordinal: 2644
            {"CS_GETOTHERACTIONFORBIDCOUNT", "GuC"},  // Count: 1, Ordinal: 2645
            {"CS_GETOTHERCAUSEFORBIDCOUNT", "HuC"},  // Count: 1, Ordinal: 2646
            {"CS_GETRDMTAGSINVOLVEDINCITATION", "IuC"},  // Count: 1, Ordinal: 2647
            {"CS_GETRULECOMPLEX", "JuC"},  // Count: 1, Ordinal: 2648
            {"CS_GETRULESBYSTRATEGY", "KuC"},  // Count: 1, Ordinal: 2649
            {"CS_GETRULESCRITERIAREPORT", "LuC"},  // Count: 1, Ordinal: 2650
            {"CS_GETRULESCRITERIAREPORT_2", "MuC"},  // Count: 1, Ordinal: 2651
            {"CS_GETSTRATEGYCAUSES", "AvC"},  // Count: 1, Ordinal: 2652
            {"CS_GETSUPPLEMENTALACTIONS", "BvC"},  // Count: 1, Ordinal: 2653
            {"CS_GETTAGS", "CvC"},  // Count: 1, Ordinal: 2654
            {"CS_GETTEST", "DvC"},  // Count: 1, Ordinal: 2655
            {"CS_INSERT_CSACTIONMASTER_1", "EvC"},  // Count: 1, Ordinal: 2656
            {"CS_INSERT_CSACTIONTAGS", "FvC"},  // Count: 1, Ordinal: 2657
            {"CS_INSERT_CSACTIONTAGSLOG", "GvC"},  // Count: 1, Ordinal: 2658
            {"CS_INSERT_CSAREAS_1", "HvC"},  // Count: 1, Ordinal: 2659
            {"CS_INSERT_CSCAUSEACTION_1", "IvC"},  // Count: 1, Ordinal: 2660
            {"CS_INSERT_CSCAUSERELATIONS_1", "JvC"},  // Count: 1, Ordinal: 2661
            {"CS_INSERT_CSCAUSERELATIONS_2", "KvC"},  // Count: 1, Ordinal: 2662
            {"CS_INSERT_CSCAUSES_1", "LvC"},  // Count: 1, Ordinal: 2663
            {"CS_INSERT_CSCITATIONCRITERIA", "MvC"},  // Count: 1, Ordinal: 2664
            {"CS_INSERT_CSCITATIONLOG_1", "AwC"},  // Count: 1, Ordinal: 2665
            {"CS_INSERT_CSDEADTIMELIST", "BwC"},  // Count: 1, Ordinal: 2666
            {"CS_INSERT_CSDEADTIMELISTLOG", "CwC"},  // Count: 1, Ordinal: 2667
            {"CS_INSERT_CSSTRATEGIES_1", "DwC"},  // Count: 1, Ordinal: 2668
            {"CS_INSERT_CSSTRATEGYCAUSES_1", "EwC"},  // Count: 1, Ordinal: 2669
            {"CS_INSERT_CSSTRATEGYCAUSES_2", "FwC"},  // Count: 1, Ordinal: 2670
            {"CS_INSERTUPDATE_CITATIONRESPONSEID", "GwC"},  // Count: 1, Ordinal: 2671
            {"CS_NEWCITATION", "HwC"},  // Count: 1, Ordinal: 2672
            {"CS_NEWCITATION_1", "IwC"},  // Count: 1, Ordinal: 2673
            {"CS_NEWRULE", "JwC"},  // Count: 1, Ordinal: 2674
            {"CS_PURGEMAIL", "KwC"},  // Count: 1, Ordinal: 2675
            {"CS_REMOVE_USERID_FROM_SUPPLEMENTALDATA", "LwC"},  // Count: 1, Ordinal: 2676
            {"CS_RULELIST", "MwC"},  // Count: 1, Ordinal: 2677
            {"CS_SELECT_CRITERIATYPENAME", "AxC"},  // Count: 1, Ordinal: 2678
            {"CS_SELECT_CSACTIONTAGS_BY_ACTIONID", "BxC"},  // Count: 1, Ordinal: 2679
            {"CS_SELECT_CSACTIONTAGS_BY_ACTIONID_TAGNAME", "CxC"},  // Count: 1, Ordinal: 2680
            {"CS_SELECT_CSACTIONTAGS_BY_TAGNAME", "DxC"},  // Count: 1, Ordinal: 2681
            {"CS_SELECT_CSCITATIONCRITERIA_BY_CITATIONID", "ExC"},  // Count: 1, Ordinal: 2682
            {"CS_SELECT_CSCITATIONS_BY_CITATIONID", "FxC"},  // Count: 1, Ordinal: 2683
            {"CS_SELECT_CSDEADTIMELIST_BY_STATUS", "GxC"},  // Count: 1, Ordinal: 2684
            {"CS_SELECT_CSDEADTIMELIST_BY_STATUS_RESETTIME", "HxC"},  // Count: 1, Ordinal: 2685
            {"CS_SELECT_CSDEADTIMELIST_BY_TAGNAME", "IxC"},  // Count: 1, Ordinal: 2686
            {"CS_SELECT_CSDEADTIMELIST_BY_TAGNAME_MAXRESETTIME", "JxC"},  // Count: 1, Ordinal: 2687
            {"CS_SELECT_CSDEADTIMELIST_DUPLICATE_TAGNAMES", "KxC"},  // Count: 1, Ordinal: 2688
            {"CS_SELECT_CSDEADTIMELISTLOG_BY_DATECREATED", "LxC"},  // Count: 1, Ordinal: 2689
            {"CS_SELECT_CSDEADTIMELISTLOG_BY_DATERESTARTED", "MxC"},  // Count: 1, Ordinal: 2690
            {"CS_SELECT_CSDEADTIMELISTLOG_BY_RESETTIME", "AyC"},  // Count: 1, Ordinal: 2691
            {"CS_SELECT_CSDEADTIMELISTLOG_BY_TAGNAME", "ByC"},  // Count: 1, Ordinal: 2692
            {"CS_SELECT_CSDEADTIMELISTLOG_BY_TAGNAME_DATECREATED", "CyC"},  // Count: 1, Ordinal: 2693
            {"CS_SELECT_CSDEADTIMELISTLOG_BY_TAGNAME_DATERESTARTED", "DyC"},  // Count: 1, Ordinal: 2694
            {"CS_SELECT_OPERATOR", "EyC"},  // Count: 1, Ordinal: 2695
            {"CS_SELECT_SERVERNAMES", "FyC"},  // Count: 1, Ordinal: 2696
            {"CS_SELECT_WATCHDOGCRITERIAUPDATESTATUS", "GyC"},  // Count: 1, Ordinal: 2697
            {"CS_UPDATE_CSACTIONMASTER_1", "HyC"},  // Count: 1, Ordinal: 2698
            {"CS_UPDATE_CSACTIONTAGS_BY_ACTIONID", "IyC"},  // Count: 1, Ordinal: 2699
            {"CS_UPDATE_CSACTIONTAGS_BY_ACTIONID_TAGNAME", "JyC"},  // Count: 1, Ordinal: 2700
            {"CS_UPDATE_CSAREAS_1", "KyC"},  // Count: 1, Ordinal: 2701
            {"CS_UPDATE_CSCAUSEACTION_1", "LyC"},  // Count: 1, Ordinal: 2702
            {"CS_UPDATE_CSCAUSERELATIONS_1", "MyC"},  // Count: 1, Ordinal: 2703
            {"CS_UPDATE_CSCAUSES_1", "AzD"},  // Count: 1, Ordinal: 2704
            {"CS_UPDATE_CSCRITERIA", "BzD"},  // Count: 1, Ordinal: 2705
            {"CS_UPDATE_CSDEADTIMELIST_BY_TAGNAME", "CzD"},  // Count: 1, Ordinal: 2706
            {"CS_UPDATE_CSDEADTIMELIST_BY_TAGNAME_DATECREATED", "DzD"},  // Count: 1, Ordinal: 2707
            {"CS_UPDATE_CSDEADTIMELIST_BY_TAGNAME_DATECREATED_ACTIONID", "EzD"},  // Count: 1, Ordinal: 2708
            {"CS_UPDATE_CSDEADTIMELIST_BY_TAGNAME_STATUS", "FzD"},  // Count: 1, Ordinal: 2709
            {"CS_UPDATE_CSSTRATEGIES_1", "GzD"},  // Count: 1, Ordinal: 2710
            {"CS_UPDATE_CSSTRATEGYCAUSES_1", "HzD"},  // Count: 1, Ordinal: 2711
            {"CS_UPDATE_PIMSCONTACTACTION", "IzD"},  // Count: 1, Ordinal: 2712
            {"CS_UPDATE_RULE_WITH_PIPES", "JzD"},  // Count: 1, Ordinal: 2713
            {"CS_UPDATE_WATCHDOGCRITERIAUPDATESTATUS", "KzD"},  // Count: 1, Ordinal: 2714
            {"CS_UPDATEACITATION", "LzD"},  // Count: 1, Ordinal: 2715
            {"CS_UPDATEACTIONINSTRUCTIONS", "MzD"},  // Count: 1, Ordinal: 2716
            {"CS_UPDATECAUSEINSTRUCTIONS", "AAD"},  // Count: 1, Ordinal: 2717
            {"CS_UPDATEMAIL", "BAD"},  // Count: 1, Ordinal: 2718
            {"CS_UPDATERULE", "CAD"},  // Count: 1, Ordinal: 2719
            {"CS_UPDATERULESTATUS", "DAD"},  // Count: 1, Ordinal: 2720
            {"CSCITATIONLOGHIST", "EAD"},  // Count: 1, Ordinal: 2721
            {"CSMAILBOXSTATUS", "FAD"},  // Count: 1, Ordinal: 2722
            {"CSRULELOGICALS", "GAD"},  // Count: 1, Ordinal: 2723
            {"CSSUPPLEMENTALACTIONS", "HAD"},  // Count: 1, Ordinal: 2724
            {"CUSTOMERSHIPTONAME", "IAD"},  // Count: 1, Ordinal: 2725
            {"CUSTOMERSHIPTONUMBER", "JAD"},  // Count: 1, Ordinal: 2726
            {"CUSTOMERSOLDTONAME", "KAD"},  // Count: 1, Ordinal: 2727
            {"CUSTOMERSOLDTONUMBER", "LAD"},  // Count: 1, Ordinal: 2728
            {"DATABASEID", "MAD"},  // Count: 1, Ordinal: 2729
            {"DATETIMECONTROLSTRATEGYLASTUPDATED", "ABD"},  // Count: 1, Ordinal: 2730
            {"DB_ID", "BBD"},  // Count: 1, Ordinal: 2731
            {"DB_NAME", "CBD"},  // Count: 1, Ordinal: 2732
            {"DEFAULT", "DBD"},  // Count: 1, Ordinal: 2733
            {"'DELETE 1'", "EBD"},  // Count: 1, Ordinal: 2734
            {"DELIVERYITEMNUMBER", "FBD"},  // Count: 1, Ordinal: 2735
            {"DELIVERYNUMBER", "GBD"},  // Count: 1, Ordinal: 2736
            {"DEST_SERVER", "HBD"},  // Count: 1, Ordinal: 2737
            {"'ELSE'", "IBD"},  // Count: 1, Ordinal: 2738
            {"EMAIL_OR_USERID", "JBD"},  // Count: 1, Ordinal: 2739
            {"EMN_SP_GETUNSENTRLINKTRANSACTIONS", "KBD"},  // Count: 1, Ordinal: 2740
            {"EMN_SP_PUSH_PRE_PIMSDB", "LBD"},  // Count: 1, Ordinal: 2741
            {"EMN_SP_SEND_BTCR", "MBD"},  // Count: 1, Ordinal: 2742
            {"EMN_SP_SEND_PHCON", "ACD"},  // Count: 1, Ordinal: 2743
            {"EMN_SP_SEND_PI_CONS", "BCD"},  // Count: 1, Ordinal: 2744
            {"EMN_SP_SEND_PI_CRST", "CCD"},  // Count: 1, Ordinal: 2745
            {"EMN_SP_SEND_PI_ORDCO", "DCD"},  // Count: 1, Ordinal: 2746
            {"EMN_SP_SEND_PIBTCR", "ECD"},  // Count: 1, Ordinal: 2747
            {"EMN_SP_SEND_PROD", "FCD"},  // Count: 1, Ordinal: 2748
            {"EMN_SP_SEND_XFER", "GCD"},  // Count: 1, Ordinal: 2749
            {"EMN_SP_SEND_YBSTAT", "HCD"},  // Count: 1, Ordinal: 2750
            {"EMN_SP_SEND_YBTCL", "ICD"},  // Count: 1, Ordinal: 2751
            {"EMN_SP_SEND_YBTCR", "JCD"},  // Count: 1, Ordinal: 2752
            {"EMN_SP_SEND_YCPYQ", "KCD"},  // Count: 1, Ordinal: 2753
            {"EMN_SP_SEND_YGMNCHG", "LCD"},  // Count: 1, Ordinal: 2754
            {"EMN_SP_SEND_YHB", "MCD"},  // Count: 1, Ordinal: 2755
            {"EMN_SP_SEND_YINVADJ", "ADD"},  // Count: 1, Ordinal: 2756
            {"EMN_SP_SEND_YPHCON", "BDD"},  // Count: 1, Ordinal: 2757
            {"EMN_SP_SEND_YPKB", "CDD"},  // Count: 1, Ordinal: 2758
            {"EMN_SP_SEND_YPOREC", "DDD"},  // Count: 1, Ordinal: 2759
            {"EMN_SP_SEND_YPRD_NPO", "EDD"},  // Count: 1, Ordinal: 2760
            {"EMN_SP_SEND_YPROD", "FDD"},  // Count: 1, Ordinal: 2761
            {"EMN_SP_SEND_YPRODADJ", "GDD"},  // Count: 1, Ordinal: 2762
            {"EMN_SP_SEND_YPRU", "HDD"},  // Count: 1, Ordinal: 2763
            {"EMN_SP_SEND_YQA01", "IDD"},  // Count: 1, Ordinal: 2764
            {"EMN_SP_SEND_YRC_STAT", "JDD"},  // Count: 1, Ordinal: 2765
            {"EMN_SP_SEND_YRCRST", "KDD"},  // Count: 1, Ordinal: 2766
            {"EMN_SP_SEND_YREWORK", "LDD"},  // Count: 1, Ordinal: 2767
            {"EMN_SP_SEND_YRINADJ", "MDD"},  // Count: 1, Ordinal: 2768
            {"EMN_SP_SEND_YRLINVAJ", "AED"},  // Count: 1, Ordinal: 2769
            {"'EMN_SP_SEND_YRLINVAJ'", "BED"},  // Count: 1, Ordinal: 2770
            {"EMN_SP_SEND_YUPT", "CED"},  // Count: 1, Ordinal: 2771
            {"EMN_SP_SEND_YXFER", "DED"},  // Count: 1, Ordinal: 2772
            {"EMN_SP_SEND_YXFER911", "EED"},  // Count: 1, Ordinal: 2773
            {"EMN_SP_SUCCESSFULRLINKTRANSACTION", "FED"},  // Count: 1, Ordinal: 2774
            {"EMN_SP_UNSUCCESSFULRLINKTRANSACTION", "GED"},  // Count: 1, Ordinal: 2775
            {"ERROR_OUT:", "HED"},  // Count: 1, Ordinal: 2776
            {"EST_BATCH_SIZE", "IED"},  // Count: 1, Ordinal: 2777
            {"'EXEC MSDB . . SP_HELP%'", "JED"},  // Count: 1, Ordinal: 2778
            {"'EXEC SP_HELP%'", "KED"},  // Count: 1, Ordinal: 2779
            {"EXPR1", "LED"},  // Count: 1, Ordinal: 2780
            {"EXPR2", "MED"},  // Count: 1, Ordinal: 2781
            {"'FAIL TO DELETE THE FILE'", "AFD"},  // Count: 1, Ordinal: 2782
            {"FIBERS", "BFD"},  // Count: 1, Ordinal: 2783
            {"FILTER_NAME", "CFD"},  // Count: 1, Ordinal: 2784
            {"GMN_NOBATCH_VALIDATION", "DFD"},  // Count: 1, Ordinal: 2785
            {"HEEL_IDENTIFIER", "EFD"},  // Count: 1, Ordinal: 2786
            {"HEEL_UOM", "FFD"},  // Count: 1, Ordinal: 2787
            {"'HELLO '", "GFD"},  // Count: 1, Ordinal: 2788
            {"'IF @@TRANCOUNT > 0'", "HFD"},  // Count: 1, Ordinal: 2789
            {"'INSERT 1'", "IFD"},  // Count: 1, Ordinal: 2790
            {"'INSERT 2'", "JFD"},  // Count: 1, Ordinal: 2791
            {"INSERT_AVAIL_RECORD", "KFD"},  // Count: 1, Ordinal: 2792
            {"INVQUANTITY", "LFD"},  // Count: 1, Ordinal: 2793
            {"ISPRODUCTION", "MFD"},  // Count: 1, Ordinal: 2794
            {"LAST_PART_IDENTIFIER", "AGD"},  // Count: 1, Ordinal: 2795
            {"LOGID'", "BGD"},  // Count: 1, Ordinal: 2796
            {"MASK_LEVEL-1", "CGD"},  // Count: 1, Ordinal: 2797
            {"MAX_NUM_USAGE_PARTS", "DGD"},  // Count: 1, Ordinal: 2798
            {"MESSAGE", "EGD"},  // Count: 1, Ordinal: 2799
            {"MOVEMENTTYPE", "FGD"},  // Count: 1, Ordinal: 2800
            {"MSGCODE", "GGD"},  // Count: 1, Ordinal: 2801
            {"MYPROC", "HGD"},  // Count: 1, Ordinal: 2802
            {"N'''", "IGD"},  // Count: 1, Ordinal: 2803
            {"N'%\"'", "JGD"},  // Count: 1, Ordinal: 2804
            {"N'%|%'", "KGD"},  // Count: 1, Ordinal: 2805
            {"N'*'", "LGD"},  // Count: 1, Ordinal: 2806
            {"N'@ACTIVITY1 = '", "MGD"},  // Count: 1, Ordinal: 2807
            {"N'@ACTIVITY1 = NULL'", "AHD"},  // Count: 1, Ordinal: 2808
            {"N'@ACTIVITY1_UNITS = '", "BHD"},  // Count: 1, Ordinal: 2809
            {"N'@ACTIVITY1_UNITS = NULL'", "CHD"},  // Count: 1, Ordinal: 2810
            {"N'@ACTIVITY1FINISHED = '", "DHD"},  // Count: 1, Ordinal: 2811
            {"N'@ACTIVITY1FINISHED = NULL'", "EHD"},  // Count: 1, Ordinal: 2812
            {"N'@ACTIVITY2 = '", "FHD"},  // Count: 1, Ordinal: 2813
            {"N'@ACTIVITY2 = NULL'", "GHD"},  // Count: 1, Ordinal: 2814
            {"N'@ACTIVITY2_UNITS = '", "HHD"},  // Count: 1, Ordinal: 2815
            {"N'@ACTIVITY2_UNITS = NULL'", "IHD"},  // Count: 1, Ordinal: 2816
            {"N'@ACTIVITY2FINISHED = '", "JHD"},  // Count: 1, Ordinal: 2817
            {"N'@ACTIVITY2FINISHED = NULL'", "KHD"},  // Count: 1, Ordinal: 2818
            {"N'@ACTIVITY3 = '", "LHD"},  // Count: 1, Ordinal: 2819
            {"N'@ACTIVITY3 = NULL'", "MHD"},  // Count: 1, Ordinal: 2820
            {"N'@ACTIVITY3_UNITS = '", "AID"},  // Count: 1, Ordinal: 2821
            {"N'@ACTIVITY3_UNITS = NULL'", "BID"},  // Count: 1, Ordinal: 2822
            {"N'@ACTIVITY3FINISHED = '", "CID"},  // Count: 1, Ordinal: 2823
            {"N'@ACTIVITY3FINISHED = NULL'", "DID"},  // Count: 1, Ordinal: 2824
            {"N'@ACTIVITY4FINISHED = '", "EID"},  // Count: 1, Ordinal: 2825
            {"N'@ACTIVITY4FINISHED = NULL'", "FID"},  // Count: 1, Ordinal: 2826
            {"N'@ACTIVITY4SIGNED = '", "GID"},  // Count: 1, Ordinal: 2827
            {"N'@ACTIVITY4SIGNED = NULL'", "HID"},  // Count: 1, Ordinal: 2828
            {"N'@ACTIVITY5 = '", "IID"},  // Count: 1, Ordinal: 2829
            {"N'@ACTIVITY5 = NULL'", "JID"},  // Count: 1, Ordinal: 2830
            {"N'@ACTIVITY5_UNITS = '", "KID"},  // Count: 1, Ordinal: 2831
            {"N'@ACTIVITY5_UNITS = NULL'", "LID"},  // Count: 1, Ordinal: 2832
            {"N'@ACTIVITY5FINISHED = '", "MID"},  // Count: 1, Ordinal: 2833
            {"N'@ACTIVITY5FINISHED = NULL'", "AJD"},  // Count: 1, Ordinal: 2834
            {"N'@ACTIVITY6FINISHED = '", "BJD"},  // Count: 1, Ordinal: 2835
            {"N'@ACTIVITY6FINISHED = NULL'", "CJD"},  // Count: 1, Ordinal: 2836
            {"N'@AREAQ NVARCHAR ( 128 ) '", "DJD"},  // Count: 1, Ordinal: 2837
            {"N'@BATCH_CHECK_SW = '", "EJD"},  // Count: 1, Ordinal: 2838
            {"N'@BATCH_CHECK_SW = NULL'", "FJD"},  // Count: 1, Ordinal: 2839
            {"N'@BOLNUMBER = '", "GJD"},  // Count: 1, Ordinal: 2840
            {"N'@BOLNUMBER = NULL'", "HJD"},  // Count: 1, Ordinal: 2841
            {"N'@BSTATUS = '", "IJD"},  // Count: 1, Ordinal: 2842
            {"N'@BSTATUS = NULL'", "JJD"},  // Count: 1, Ordinal: 2843
            {"N'@CLASSIFICATION = '", "KJD"},  // Count: 1, Ordinal: 2844
            {"N'@CLASSIFICATION = NULL'", "LJD"},  // Count: 1, Ordinal: 2845
            {"N'@CLEAR_RESERVATIONS = '", "MJD"},  // Count: 1, Ordinal: 2846
            {"N'@CLEAR_RESERVATIONS = NULL'", "AKD"},  // Count: 1, Ordinal: 2847
            {"N'@CONFIRMATION_SHORT_TEXT = '", "BKD"},  // Count: 1, Ordinal: 2848
            {"N'@CONFIRMATION_SHORT_TEXT = NULL'", "CKD"},  // Count: 1, Ordinal: 2849
            {"N'@CONFIRMATIONSHORTTEXT = '", "DKD"},  // Count: 1, Ordinal: 2850
            {"N'@CONFIRMATIONSHORTTEXT = NULL'", "EKD"},  // Count: 1, Ordinal: 2851
            {"N'@CONTAINER_ID = '", "FKD"},  // Count: 1, Ordinal: 2852
            {"N'@CONTAINER_ID = NULL'", "GKD"},  // Count: 1, Ordinal: 2853
            {"N'@CPY_BATCH = '", "HKD"},  // Count: 1, Ordinal: 2854
            {"N'@CPY_BATCH = NULL'", "IKD"},  // Count: 1, Ordinal: 2855
            {"N'@CPY_MATERIAL = '", "JKD"},  // Count: 1, Ordinal: 2856
            {"N'@CPY_MATERIAL = NULL'", "KKD"},  // Count: 1, Ordinal: 2857
            {"N'@CREATE_ONLY = '", "LKD"},  // Count: 1, Ordinal: 2858
            {"N'@CREATE_ONLY = NULL'", "MKD"},  // Count: 1, Ordinal: 2859
            {"N'@CUSTOMER = '", "ALD"},  // Count: 1, Ordinal: 2860
            {"N'@CUSTOMER = NULL'", "BLD"},  // Count: 1, Ordinal: 2861
            {"N'@DELIVERY = '", "CLD"},  // Count: 1, Ordinal: 2862
            {"N'@DELIVERY = NULL'", "DLD"},  // Count: 1, Ordinal: 2863
            {"N'@DESCRIPTION = '", "ELD"},  // Count: 1, Ordinal: 2864
            {"N'@DESCRIPTION = NULL'", "FLD"},  // Count: 1, Ordinal: 2865
            {"N'@DURATION = '", "GLD"},  // Count: 1, Ordinal: 2866
            {"N'@DURATION = NULL'", "HLD"},  // Count: 1, Ordinal: 2867
            {"N'@END_DATE = '", "ILD"},  // Count: 1, Ordinal: 2868
            {"N'@END_DATE = NULL'", "JLD"},  // Count: 1, Ordinal: 2869
            {"N'@END_TIME = '", "KLD"},  // Count: 1, Ordinal: 2870
            {"N'@END_TIME = NULL'", "LLD"},  // Count: 1, Ordinal: 2871
            {"N'@EVENTDATE = '", "MLD"},  // Count: 1, Ordinal: 2872
            {"N'@EVENTDATE = NULL'", "AMD"},  // Count: 1, Ordinal: 2873
            {"N'@EVENTTIME = '", "BMD"},  // Count: 1, Ordinal: 2874
            {"N'@EVENTTIME = NULL'", "CMD"},  // Count: 1, Ordinal: 2875
            {"N'@FINAL_CONFIRMATION = '", "DMD"},  // Count: 1, Ordinal: 2876
            {"N'@FINAL_CONFIRMATION = NULL'", "EMD"},  // Count: 1, Ordinal: 2877
            {"N'@FINAL_ISSUE = '", "FMD"},  // Count: 1, Ordinal: 2878
            {"N'@FINAL_ISSUE = NULL'", "GMD"},  // Count: 1, Ordinal: 2879
            {"N'@FINALISSUE = '", "HMD"},  // Count: 1, Ordinal: 2880
            {"N'@FINALISSUE = NULL'", "IMD"},  // Count: 1, Ordinal: 2881
            {"N'@FRM_BATCH = '", "JMD"},  // Count: 1, Ordinal: 2882
            {"N'@FRM_BATCH = NULL'", "KMD"},  // Count: 1, Ordinal: 2883
            {"N'@FRM_MATERIAL = '", "LMD"},  // Count: 1, Ordinal: 2884
            {"N'@FRM_MATERIAL = NULL'", "MMD"},  // Count: 1, Ordinal: 2885
            {"N'@FROMRECIPEID = '", "AND"},  // Count: 1, Ordinal: 2886
            {"N'@FROMRECIPEID = NULL'", "BND"},  // Count: 1, Ordinal: 2887
            {"N'@GENERATECONSUMPTIONBIT = '", "CND"},  // Count: 1, Ordinal: 2888
            {"N'@GENERATECONSUMPTIONBIT = NULL'", "DND"},  // Count: 1, Ordinal: 2889
            {"N'@GMN = '", "END"},  // Count: 1, Ordinal: 2890
            {"N'@GMN = NULL'", "FND"},  // Count: 1, Ordinal: 2891
            {"N'@HEADERTEXT = '", "GND"},  // Count: 1, Ordinal: 2892
            {"N'@HEADERTEXT = NULL'", "HND"},  // Count: 1, Ordinal: 2893
            {"N'@INSPECTION_ORIGIN = '", "IND"},  // Count: 1, Ordinal: 2894
            {"N'@INSPECTION_ORIGIN = NULL'", "JND"},  // Count: 1, Ordinal: 2895
            {"N'@INSPECTION_SHORT_TEXT = '", "KND"},  // Count: 1, Ordinal: 2896
            {"N'@INSPECTION_SHORT_TEXT = NULL'", "LND"},  // Count: 1, Ordinal: 2897
            {"N'@INSPLOT = '", "MND"},  // Count: 1, Ordinal: 2898
            {"N'@INSPLOT = NULL'", "AOD"},  // Count: 1, Ordinal: 2899
            {"N'@INSPLOTORIGIN = '", "BOD"},  // Count: 1, Ordinal: 2900
            {"N'@INSPLOTORIGIN = NULL'", "COD"},  // Count: 1, Ordinal: 2901
            {"N'@INSPORIGIN = '", "DOD"},  // Count: 1, Ordinal: 2902
            {"N'@INSPORIGIN = NULL'", "EOD"},  // Count: 1, Ordinal: 2903
            {"N'@INSPTYPE = '", "FOD"},  // Count: 1, Ordinal: 2904
            {"N'@INSPTYPE = NULL'", "GOD"},  // Count: 1, Ordinal: 2905
            {"N'@LIFNR = '", "HOD"},  // Count: 1, Ordinal: 2906
            {"N'@LIFNR = NULL'", "IOD"},  // Count: 1, Ordinal: 2907
            {"N'@MAINT_WINDOW = '", "JOD"},  // Count: 1, Ordinal: 2908
            {"N'@MAINT_WINDOW = NULL'", "KOD"},  // Count: 1, Ordinal: 2909
            {"N'@MATERIAL_DOCUMENT = '", "LOD"},  // Count: 1, Ordinal: 2910
            {"N'@MATERIAL_DOCUMENT = NULL'", "MOD"},  // Count: 1, Ordinal: 2911
            {"N'@MATERIAL_DOCUMENT_YEAR = '", "APD"},  // Count: 1, Ordinal: 2912
            {"N'@MATERIAL_DOCUMENT_YEAR = NULL'", "BPD"},  // Count: 1, Ordinal: 2913
            {"N'@MATERIAL_ID = '", "CPD"},  // Count: 1, Ordinal: 2914
            {"N'@MATERIAL_ID = NULL'", "DPD"},  // Count: 1, Ordinal: 2915
            {"N'@MD013_INV_STATUS = '", "EPD"},  // Count: 1, Ordinal: 2916
            {"N'@MD013_INV_STATUS = NULL'", "FPD"},  // Count: 1, Ordinal: 2917
            {"N'@MF_BATCH = '", "GPD"},  // Count: 1, Ordinal: 2918
            {"N'@MF_BATCH = NULL'", "HPD"},  // Count: 1, Ordinal: 2919
            {"N'@MF_MATERIAL = '", "IPD"},  // Count: 1, Ordinal: 2920
            {"N'@MF_MATERIAL = NULL'", "JPD"},  // Count: 1, Ordinal: 2921
            {"N'@NAME21 = '", "KPD"},  // Count: 1, Ordinal: 2922
            {"N'@NAME21 = NULL'", "LPD"},  // Count: 1, Ordinal: 2923
            {"N'@NAME22 = '", "MPD"},  // Count: 1, Ordinal: 2924
            {"N'@NAME22 = NULL'", "AQD"},  // Count: 1, Ordinal: 2925
            {"N'@NAME23 = '", "BQD"},  // Count: 1, Ordinal: 2926
            {"N'@NAME23 = NULL'", "CQD"},  // Count: 1, Ordinal: 2927
            {"N'@NAME24 = '", "DQD"},  // Count: 1, Ordinal: 2928
            {"N'@NAME24 = NULL'", "EQD"},  // Count: 1, Ordinal: 2929
            {"N'@NAME25 = '", "FQD"},  // Count: 1, Ordinal: 2930
            {"N'@NAME25 = NULL'", "GQD"},  // Count: 1, Ordinal: 2931
            {"N'@NAME26 = '", "HQD"},  // Count: 1, Ordinal: 2932
            {"N'@NAME26 = NULL'", "IQD"},  // Count: 1, Ordinal: 2933
            {"N'@NAME27 = '", "JQD"},  // Count: 1, Ordinal: 2934
            {"N'@NAME27 = NULL'", "KQD"},  // Count: 1, Ordinal: 2935
            {"N'@NAME28 = '", "LQD"},  // Count: 1, Ordinal: 2936
            {"N'@NAME28 = NULL'", "MQD"},  // Count: 1, Ordinal: 2937
            {"N'@NAME29 = '", "ARD"},  // Count: 1, Ordinal: 2938
            {"N'@NAME29 = NULL'", "BRD"},  // Count: 1, Ordinal: 2939
            {"N'@NAME30 = '", "CRD"},  // Count: 1, Ordinal: 2940
            {"N'@NAME30 = NULL'", "DRD"},  // Count: 1, Ordinal: 2941
            {"N'@NEW_POSTING_DATE = '", "ERD"},  // Count: 1, Ordinal: 2942
            {"N'@NEW_POSTING_DATE = NULL'", "FRD"},  // Count: 1, Ordinal: 2943
            {"N'@NEWPOSTTIME = '", "GRD"},  // Count: 1, Ordinal: 2944
            {"N'@NEWPOSTTIME = NULL'", "HRD"},  // Count: 1, Ordinal: 2945
            {"N'@OPERATION_PRM = '", "IRD"},  // Count: 1, Ordinal: 2946
            {"N'@OPERATION_PRM = NULL'", "JRD"},  // Count: 1, Ordinal: 2947
            {"N'@OUTAGE_END_DATE = '", "KRD"},  // Count: 1, Ordinal: 2948
            {"N'@OUTAGE_END_DATE = NULL'", "LRD"},  // Count: 1, Ordinal: 2949
            {"N'@OUTAGE_END_TIME = '", "MRD"},  // Count: 1, Ordinal: 2950
            {"N'@OUTAGE_END_TIME = NULL'", "ASD"},  // Count: 1, Ordinal: 2951
            {"N'@OUTAGE_START_DATE = '", "BSD"},  // Count: 1, Ordinal: 2952
            {"N'@OUTAGE_START_DATE = NULL'", "CSD"},  // Count: 1, Ordinal: 2953
            {"N'@OUTAGE_START_TIME = '", "DSD"},  // Count: 1, Ordinal: 2954
            {"N'@OUTAGE_START_TIME = NULL'", "ESD"},  // Count: 1, Ordinal: 2955
            {"N'@PERFORM_QA01 = '", "FSD"},  // Count: 1, Ordinal: 2956
            {"N'@PERFORM_QA01 = NULL'", "GSD"},  // Count: 1, Ordinal: 2957
            {"N'@PERFORM_WHEN_INV_EXISTS = '", "HSD"},  // Count: 1, Ordinal: 2958
            {"N'@PERFORM_WHEN_INV_EXISTS = NULL'", "ISD"},  // Count: 1, Ordinal: 2959
            {"N'@PHASE_RESOURCE = '", "JSD"},  // Count: 1, Ordinal: 2960
            {"N'@PHASE_RESOURCE = NULL'", "KSD"},  // Count: 1, Ordinal: 2961
            {"N'@PHASE_RESOURCE_PRM = '", "LSD"},  // Count: 1, Ordinal: 2962
            {"N'@PHASE_RESOURCE_PRM = NULL'", "MSD"},  // Count: 1, Ordinal: 2963
            {"N'@PICONS_FROM_MATL_BATCH = '", "ATD"},  // Count: 1, Ordinal: 2964
            {"N'@PICONS_FROM_MATL_BATCH = NULL'", "BTD"},  // Count: 1, Ordinal: 2965
            {"N'@PKG_BATCH = '", "CTD"},  // Count: 1, Ordinal: 2966
            {"N'@PKG_BATCH = NULL'", "DTD"},  // Count: 1, Ordinal: 2967
            {"N'@PKG_ID = '", "ETD"},  // Count: 1, Ordinal: 2968
            {"N'@PKG_ID = NULL'", "FTD"},  // Count: 1, Ordinal: 2969
            {"N'@PKG_MATERIAL = '", "GTD"},  // Count: 1, Ordinal: 2970
            {"N'@PKG_MATERIAL = NULL'", "HTD"},  // Count: 1, Ordinal: 2971
            {"N'@PKG_STATUS = '", "ITD"},  // Count: 1, Ordinal: 2972
            {"N'@PKG_STATUS = NULL'", "JTD"},  // Count: 1, Ordinal: 2973
            {"N'@PKG_TYP = '", "KTD"},  // Count: 1, Ordinal: 2974
            {"N'@PKG_TYP = NULL'", "LTD"},  // Count: 1, Ordinal: 2975
            {"N'@PLANT_ID_PRM = '", "MTD"},  // Count: 1, Ordinal: 2976
            {"N'@PLANT_ID_PRM = NULL'", "AUD"},  // Count: 1, Ordinal: 2977
            {"N'@PLANTID = '", "BUD"},  // Count: 1, Ordinal: 2978
            {"N'@PLANTID = NULL'", "CUD"},  // Count: 1, Ordinal: 2979
            {"N'@PO_NUMBER = '", "DUD"},  // Count: 1, Ordinal: 2980
            {"N'@PO_NUMBER = NULL'", "EUD"},  // Count: 1, Ordinal: 2981
            {"N'@POCOMPLETE = '", "FUD"},  // Count: 1, Ordinal: 2982
            {"N'@POCOMPLETE = NULL'", "GUD"},  // Count: 1, Ordinal: 2983
            {"N'@PONM01 = '", "HUD"},  // Count: 1, Ordinal: 2984
            {"N'@PONM01 = NULL'", "IUD"},  // Count: 1, Ordinal: 2985
            {"N'@PONM02 = '", "JUD"},  // Count: 1, Ordinal: 2986
            {"N'@PONM02 = NULL'", "KUD"},  // Count: 1, Ordinal: 2987
            {"N'@PONM03 = '", "LUD"},  // Count: 1, Ordinal: 2988
            {"N'@PONM03 = NULL'", "MUD"},  // Count: 1, Ordinal: 2989
            {"N'@PONM04 = '", "AVD"},  // Count: 1, Ordinal: 2990
            {"N'@PONM04 = NULL'", "BVD"},  // Count: 1, Ordinal: 2991
            {"N'@PONM05 = '", "CVD"},  // Count: 1, Ordinal: 2992
            {"N'@PONM05 = NULL'", "DVD"},  // Count: 1, Ordinal: 2993
            {"N'@PONM06 = '", "EVD"},  // Count: 1, Ordinal: 2994
            {"N'@PONM06 = NULL'", "FVD"},  // Count: 1, Ordinal: 2995
            {"N'@PONM07 = '", "GVD"},  // Count: 1, Ordinal: 2996
            {"N'@PONM07 = NULL'", "HVD"},  // Count: 1, Ordinal: 2997
            {"N'@PONM08 = '", "IVD"},  // Count: 1, Ordinal: 2998
            {"N'@PONM08 = NULL'", "JVD"},  // Count: 1, Ordinal: 2999
            {"N'@PONM09 = '", "KVD"},  // Count: 1, Ordinal: 3000
            {"N'@PONM09 = NULL'", "LVD"},  // Count: 1, Ordinal: 3001
            {"N'@PONM10 = '", "MVD"},  // Count: 1, Ordinal: 3002
            {"N'@PONM10 = NULL'", "AWD"},  // Count: 1, Ordinal: 3003
            {"N'@PONM11 = '", "BWD"},  // Count: 1, Ordinal: 3004
            {"N'@PONM11 = NULL'", "CWD"},  // Count: 1, Ordinal: 3005
            {"N'@PONM12 = '", "DWD"},  // Count: 1, Ordinal: 3006
            {"N'@PONM12 = NULL'", "EWD"},  // Count: 1, Ordinal: 3007
            {"N'@PONM13 = '", "FWD"},  // Count: 1, Ordinal: 3008
            {"N'@PONM13 = NULL'", "GWD"},  // Count: 1, Ordinal: 3009
            {"N'@PONM14 = '", "HWD"},  // Count: 1, Ordinal: 3010
            {"N'@PONM14 = NULL'", "IWD"},  // Count: 1, Ordinal: 3011
            {"N'@PONM15 = '", "JWD"},  // Count: 1, Ordinal: 3012
            {"N'@PONM15 = NULL'", "KWD"},  // Count: 1, Ordinal: 3013
            {"N'@PONM16 = '", "LWD"},  // Count: 1, Ordinal: 3014
            {"N'@PONM16 = NULL'", "MWD"},  // Count: 1, Ordinal: 3015
            {"N'@PONM17 = '", "AXD"},  // Count: 1, Ordinal: 3016
            {"N'@PONM17 = NULL'", "BXD"},  // Count: 1, Ordinal: 3017
            {"N'@PONM18 = '", "CXD"},  // Count: 1, Ordinal: 3018
            {"N'@PONM18 = NULL'", "DXD"},  // Count: 1, Ordinal: 3019
            {"N'@PONM19 = '", "EXD"},  // Count: 1, Ordinal: 3020
            {"N'@PONM19 = NULL'", "FXD"},  // Count: 1, Ordinal: 3021
            {"N'@PONM20 = '", "GXD"},  // Count: 1, Ordinal: 3022
            {"N'@PONM20 = NULL'", "HXD"},  // Count: 1, Ordinal: 3023
            {"N'@PONM21 = '", "IXD"},  // Count: 1, Ordinal: 3024
            {"N'@PONM21 = NULL'", "JXD"},  // Count: 1, Ordinal: 3025
            {"N'@PONM22 = '", "KXD"},  // Count: 1, Ordinal: 3026
            {"N'@PONM22 = NULL'", "LXD"},  // Count: 1, Ordinal: 3027
            {"N'@PONM23 = '", "MXD"},  // Count: 1, Ordinal: 3028
            {"N'@PONM23 = NULL'", "AYD"},  // Count: 1, Ordinal: 3029
            {"N'@PONM24 = '", "BYD"},  // Count: 1, Ordinal: 3030
            {"N'@PONM24 = NULL'", "CYD"},  // Count: 1, Ordinal: 3031
            {"N'@PONM25 = '", "DYD"},  // Count: 1, Ordinal: 3032
            {"N'@PONM25 = NULL'", "EYD"},  // Count: 1, Ordinal: 3033
            {"N'@PONM26 = '", "FYD"},  // Count: 1, Ordinal: 3034
            {"N'@PONM26 = NULL'", "GYD"},  // Count: 1, Ordinal: 3035
            {"N'@PONM27 = '", "HYD"},  // Count: 1, Ordinal: 3036
            {"N'@PONM27 = NULL'", "IYD"},  // Count: 1, Ordinal: 3037
            {"N'@PONM28 = '", "JYD"},  // Count: 1, Ordinal: 3038
            {"N'@PONM28 = NULL'", "KYD"},  // Count: 1, Ordinal: 3039
            {"N'@PONM29 = '", "LYD"},  // Count: 1, Ordinal: 3040
            {"N'@PONM29 = NULL'", "MYD"},  // Count: 1, Ordinal: 3041
            {"N'@PONM30 = '", "AZD"},  // Count: 1, Ordinal: 3042
            {"N'@PONM30 = NULL'", "BZD"},  // Count: 1, Ordinal: 3043
            {"N'@POSTTIME = '", "CZD"},  // Count: 1, Ordinal: 3044
            {"N'@POSTTIME = NULL'", "DZD"},  // Count: 1, Ordinal: 3045
            {"N'@PPPI_PLANT_OF_BATCH = '", "EZD"},  // Count: 1, Ordinal: 3046
            {"N'@PPPI_PLANT_OF_BATCH = NULL'", "FZD"},  // Count: 1, Ordinal: 3047
            {"N'@PROCESS_ORDER_PRM = '", "GZD"},  // Count: 1, Ordinal: 3048
            {"N'@PROCESS_ORDER_PRM = NULL'", "HZD"},  // Count: 1, Ordinal: 3049
            {"N'@PROD_SCHED_PROF = '", "IZD"},  // Count: 1, Ordinal: 3050
            {"N'@PROD_SCHED_PROF = NULL'", "JZD"},  // Count: 1, Ordinal: 3051
            {"N'@PRODUCT_BATCH_ID = '", "KZD"},  // Count: 1, Ordinal: 3052
            {"N'@PRODUCT_BATCH_ID = NULL'", "LZD"},  // Count: 1, Ordinal: 3053
            {"N'@PRODUCT_GMN = '", "MZD"},  // Count: 1, Ordinal: 3054
            {"N'@PRODUCT_GMN = NULL'", "AaD"},  // Count: 1, Ordinal: 3055
            {"N'@PRODUCTION_DATE = '", "BaD"},  // Count: 1, Ordinal: 3056
            {"N'@PRODUCTION_DATE = NULL'", "CaD"},  // Count: 1, Ordinal: 3057
            {"N'@PRODUCTION_VERSION = '", "DaD"},  // Count: 1, Ordinal: 3058
            {"N'@PRODUCTION_VERSION = NULL'", "EaD"},  // Count: 1, Ordinal: 3059
            {"N'@QUANTITYTYPE = '", "FaD"},  // Count: 1, Ordinal: 3060
            {"N'@QUANTITYTYPE = NULL'", "GaD"},  // Count: 1, Ordinal: 3061
            {"N'@REFNUMBER = '", "HaD"},  // Count: 1, Ordinal: 3062
            {"N'@REFNUMBER = NULL'", "IaD"},  // Count: 1, Ordinal: 3063
            {"N'@SCHEDULED = '", "JaD"},  // Count: 1, Ordinal: 3064
            {"N'@SCHEDULED = NULL'", "KaD"},  // Count: 1, Ordinal: 3065
            {"N'@SCHEDULING_TYPE = '", "LaD"},  // Count: 1, Ordinal: 3066
            {"N'@SCHEDULING_TYPE = NULL'", "MaD"},  // Count: 1, Ordinal: 3067
            {"N'@SCRAPTOCONFIRM = '", "AbD"},  // Count: 1, Ordinal: 3068
            {"N'@SCRAPTOCONFIRM = NULL'", "BbD"},  // Count: 1, Ordinal: 3069
            {"N'@SECONDARY_RESOURCE = '", "CbD"},  // Count: 1, Ordinal: 3070
            {"N'@SECONDARY_RESOURCE = NULL'", "DbD"},  // Count: 1, Ordinal: 3071
            {"N'@SLOC = '", "EbD"},  // Count: 1, Ordinal: 3072
            {"N'@SLOC = NULL'", "FbD"},  // Count: 1, Ordinal: 3073
            {"N'@START_DATE = '", "GbD"},  // Count: 1, Ordinal: 3074
            {"N'@START_DATE = NULL'", "HbD"},  // Count: 1, Ordinal: 3075
            {"N'@START_TIME = '", "IbD"},  // Count: 1, Ordinal: 3076
            {"N'@START_TIME = NULL'", "JbD"},  // Count: 1, Ordinal: 3077
            {"N'@SUPPLIER = '", "KbD"},  // Count: 1, Ordinal: 3078
            {"N'@SUPPLIER = NULL'", "LbD"},  // Count: 1, Ordinal: 3079
            {"N'@SYSTEM_ID = '", "MbD"},  // Count: 1, Ordinal: 3080
            {"N'@SYSTEM_ID = NULL'", "AcD"},  // Count: 1, Ordinal: 3081
            {"N'@TO_STORAGE_LOCATION = '", "BcD"},  // Count: 1, Ordinal: 3082
            {"N'@TO_STORAGE_LOCATION = NULL'", "CcD"},  // Count: 1, Ordinal: 3083
            {"N'@TORECIPEID = '", "DcD"},  // Count: 1, Ordinal: 3084
            {"N'@TORECIPEID = NULL'", "EcD"},  // Count: 1, Ordinal: 3085
            {"N'@UNITS = '", "FcD"},  // Count: 1, Ordinal: 3086
            {"N'@UNITS = NULL'", "GcD"},  // Count: 1, Ordinal: 3087
            {"N'@USG_BLOCKED_ALLOWED = '", "HcD"},  // Count: 1, Ordinal: 3088
            {"N'@USG_BLOCKED_ALLOWED = NULL'", "IcD"},  // Count: 1, Ordinal: 3089
            {"N'@VALUE10 = '", "JcD"},  // Count: 1, Ordinal: 3090
            {"N'@VALUE10 = NULL'", "KcD"},  // Count: 1, Ordinal: 3091
            {"N'@VALUE20 = '", "LcD"},  // Count: 1, Ordinal: 3092
            {"N'@VALUE20 = NULL'", "McD"},  // Count: 1, Ordinal: 3093
            {"N'@VALUE21 = '", "AdD"},  // Count: 1, Ordinal: 3094
            {"N'@VALUE21 = NULL'", "BdD"},  // Count: 1, Ordinal: 3095
            {"N'@VALUE22 = '", "CdD"},  // Count: 1, Ordinal: 3096
            {"N'@VALUE22 = NULL'", "DdD"},  // Count: 1, Ordinal: 3097
            {"N'@VALUE23 = '", "EdD"},  // Count: 1, Ordinal: 3098
            {"N'@VALUE23 = NULL'", "FdD"},  // Count: 1, Ordinal: 3099
            {"N'@VALUE24 = '", "GdD"},  // Count: 1, Ordinal: 3100
            {"N'@VALUE24 = NULL'", "HdD"},  // Count: 1, Ordinal: 3101
            {"N'@VALUE25 = '", "IdD"},  // Count: 1, Ordinal: 3102
            {"N'@VALUE25 = NULL'", "JdD"},  // Count: 1, Ordinal: 3103
            {"N'@VALUE26 = '", "KdD"},  // Count: 1, Ordinal: 3104
            {"N'@VALUE26 = NULL'", "LdD"},  // Count: 1, Ordinal: 3105
            {"N'@VALUE27 = '", "MdD"},  // Count: 1, Ordinal: 3106
            {"N'@VALUE27 = NULL'", "AeD"},  // Count: 1, Ordinal: 3107
            {"N'@VALUE28 = '", "BeD"},  // Count: 1, Ordinal: 3108
            {"N'@VALUE28 = NULL'", "CeD"},  // Count: 1, Ordinal: 3109
            {"N'@VALUE29 = '", "DeD"},  // Count: 1, Ordinal: 3110
            {"N'@VALUE29 = NULL'", "EeD"},  // Count: 1, Ordinal: 3111
            {"N'@VALUE30 = '", "FeD"},  // Count: 1, Ordinal: 3112
            {"N'@VALUE30 = NULL'", "GeD"},  // Count: 1, Ordinal: 3113
            {"N'@VENDOR = '", "HeD"},  // Count: 1, Ordinal: 3114
            {"N'@VENDOR = NULL'", "IeD"},  // Count: 1, Ordinal: 3115
            {"N'0'", "JeD"},  // Count: 1, Ordinal: 3116
            {"N'89'", "KeD"},  // Count: 1, Ordinal: 3117
            {"N'A'", "LeD"},  // Count: 1, Ordinal: 3118
            {"N'AND A . CAUSEID = B . CAUSEID'", "MeD"},  // Count: 1, Ordinal: 3119
            {"N'BULK'", "AfD"},  // Count: 1, Ordinal: 3120
            {"N'C:\\TEST . XML'", "BfD"},  // Count: 1, Ordinal: 3121
            {"N'CLOSED'", "CfD"},  // Count: 1, Ordinal: 3122
            {"N'CREATE DATABASE NAME FILTER FAILED'", "DfD"},  // Count: 1, Ordinal: 3123
            {"N'CREATE EXLUDEPROGRAM FILTER FAILED'", "EfD"},  // Count: 1, Ordinal: 3124
            {"N'CREATE INLUDEPROGRAM FILTER FAILED'", "FfD"},  // Count: 1, Ordinal: 3125
            {"N'DEL \"'", "GfD"},  // Count: 1, Ordinal: 3126
            {"N'DELETE FROM CSMAILBOXES WHERE TIMESTAMP < = '''", "HfD"},  // Count: 1, Ordinal: 3127
            {"N'DELETING -'", "IfD"},  // Count: 1, Ordinal: 3128
            {"N'''EMAIL'''", "JfD"},  // Count: 1, Ordinal: 3129
            {"N'EMN_SP_SEND_BTCR'", "KfD"},  // Count: 1, Ordinal: 3130
            {"N'EMN_SP_SEND_PHCON'", "LfD"},  // Count: 1, Ordinal: 3131
            {"N'EMN_SP_SEND_PI_CONS'", "MfD"},  // Count: 1, Ordinal: 3132
            {"N'EMN_SP_SEND_PI_CRST'", "AgD"},  // Count: 1, Ordinal: 3133
            {"N'EMN_SP_SEND_PI_ORDCO'", "BgD"},  // Count: 1, Ordinal: 3134
            {"N'EMN_SP_SEND_PIBTCR'", "CgD"},  // Count: 1, Ordinal: 3135
            {"N'EMN_SP_SEND_PROD'", "DgD"},  // Count: 1, Ordinal: 3136
            {"N'EMN_SP_SEND_XFER'", "EgD"},  // Count: 1, Ordinal: 3137
            {"N'EMN_SP_SEND_YBSTAT'", "FgD"},  // Count: 1, Ordinal: 3138
            {"N'EMN_SP_SEND_YBTCL'", "GgD"},  // Count: 1, Ordinal: 3139
            {"N'EMN_SP_SEND_YBTCR'", "HgD"},  // Count: 1, Ordinal: 3140
            {"N'EMN_SP_SEND_YCPYQ'", "IgD"},  // Count: 1, Ordinal: 3141
            {"N'EMN_SP_SEND_YGMNCHG'", "JgD"},  // Count: 1, Ordinal: 3142
            {"N'EMN_SP_SEND_YHB'", "KgD"},  // Count: 1, Ordinal: 3143
            {"N'EMN_SP_SEND_YINVADJ'", "LgD"},  // Count: 1, Ordinal: 3144
            {"N'EMN_SP_SEND_YPHCON'", "MgD"},  // Count: 1, Ordinal: 3145
            {"N'EMN_SP_SEND_YPKB'", "AhD"},  // Count: 1, Ordinal: 3146
            {"N'EMN_SP_SEND_YPOREC'", "BhD"},  // Count: 1, Ordinal: 3147
            {"N'EMN_SP_SEND_YPRD_NPO'", "ChD"},  // Count: 1, Ordinal: 3148
            {"N'EMN_SP_SEND_YPROD'", "DhD"},  // Count: 1, Ordinal: 3149
            {"N'EMN_SP_SEND_YPRODADJ'", "EhD"},  // Count: 1, Ordinal: 3150
            {"N'EMN_SP_SEND_YPRU'", "FhD"},  // Count: 1, Ordinal: 3151
            {"N'EMN_SP_SEND_YQA01'", "GhD"},  // Count: 1, Ordinal: 3152
            {"N'EMN_SP_SEND_YRC_STAT'", "HhD"},  // Count: 1, Ordinal: 3153
            {"N'EMN_SP_SEND_YRCRST'", "IhD"},  // Count: 1, Ordinal: 3154
            {"N'EMN_SP_SEND_YREWORK'", "JhD"},  // Count: 1, Ordinal: 3155
            {"N'EMN_SP_SEND_YRINADJ'", "KhD"},  // Count: 1, Ordinal: 3156
            {"N'EMN_SP_SEND_YUPT'", "LhD"},  // Count: 1, Ordinal: 3157
            {"N'EMN_SP_SEND_YXFER'", "MhD"},  // Count: 1, Ordinal: 3158
            {"N'EMN_SP_SEND_YXFER911'", "AiD"},  // Count: 1, Ordinal: 3159
            {"N'EQUAL'", "BiD"},  // Count: 1, Ordinal: 3160
            {"NEW_PRODUCT", "CiD"},  // Count: 1, Ordinal: 3161
            {"NEW_STATE", "DiD"},  // Count: 1, Ordinal: 3162
            {"N'FOUND A DUPLICATE'", "EiD"},  // Count: 1, Ordinal: 3163
            {"N'INITIALS'", "FiD"},  // Count: 1, Ordinal: 3164
            {"N'INNER JOIN CSAREAS AS AREAS ON AREAS . AREAID = RULES . AREAID ORDER BY AREAS . AREANAME'", "GiD"},  // Count: 1, Ordinal: 3165
            {"N'INSERT INTO '", "HiD"},  // Count: 1, Ordinal: 3166
            {"N'M . VALUE ", "IiD"},  // Count: 1, Ordinal: 3167
            {"N'MASK'", "JiD"},  // Count: 1, Ordinal: 3168
            {"N'NOT_APPLICABLE'", "KiD"},  // Count: 1, Ordinal: 3169
            {"N'OPEN'", "LiD"},  // Count: 1, Ordinal: 3170
            {"N'PI_BT_CR'", "MiD"},  // Count: 1, Ordinal: 3171
            {"N'PI_CONS'", "AjD"},  // Count: 1, Ordinal: 3172
            {"N'PI_ORDCO'", "BjD"},  // Count: 1, Ordinal: 3173
            {"N'PI_PHCON'", "CjD"},  // Count: 1, Ordinal: 3174
            {"N'PI_PROD'", "DjD"},  // Count: 1, Ordinal: 3175
            {"N'PKG'", "EjD"},  // Count: 1, Ordinal: 3176
            {"N'''PRINT'''", "FjD"},  // Count: 1, Ordinal: 3177
            {"N'RENAME FAIILED - CAN NOT DELETE OLD UNIT'", "GjD"},  // Count: 1, Ordinal: 3178
            {"N'RENAME FAILED - CAN NOT DELETE UNIT'", "HjD"},  // Count: 1, Ordinal: 3179
            {"N'RENAME FAILED - CAN NOT INSERT NEW UNIT'", "IjD"},  // Count: 1, Ordinal: 3180
            {"N'REORKED'", "JjD"},  // Count: 1, Ordinal: 3181
            {"N'RULES . EVALUATIONDELAY ", "KjD"},  // Count: 1, Ordinal: 3182
            {"N'S'", "LjD"},  // Count: 1, Ordinal: 3183
            {"N'SELECT * FROM CSAREAS ORDER BY AREANAME'", "MjD"},  // Count: 1, Ordinal: 3184
            {"N'SELECT * FROM CSAREAS WHERE AREANAME = '''", "AkD"},  // Count: 1, Ordinal: 3185
            {"N'SELECT * FROM CSSTRATEGIES ORDER BY DESCRIPTION'", "BkD"},  // Count: 1, Ordinal: 3186
            {"N'SELECT * FROM CSTYPES ORDER BY TYPES'", "CkD"},  // Count: 1, Ordinal: 3187
            {"N'SELECT CA . CAUSEID ", "DkD"},  // Count: 1, Ordinal: 3188
            {"N'SELECT CHILDCAUSEID FROM CSCAUSERELATIONS WHERE PARENTCAUSEID = '", "EkD"},  // Count: 1, Ordinal: 3189
            {"N'SELECT DISTINCT A . CAUSEID ", "FkD"},  // Count: 1, Ordinal: 3190
            {"N'SELECT DISTINCT AREAS . AREANAME ", "GkD"},  // Count: 1, Ordinal: 3191
            {"N'SELECT DISTINCT CAUSACT . ACTIONID ", "HkD"},  // Count: 1, Ordinal: 3192
            {"N'SELECT DISTINCT CAUSEID ", "IkD"},  // Count: 1, Ordinal: 3193
            {"N'SELECT DISTINCT CHILDCAUSEID FROM CSCAUSERELATIONS WHERE PARENTCAUSEID = '", "JkD"},  // Count: 1, Ordinal: 3194
            {"N'SELECT DISTINCT CITATIONS . CITATIONID ", "KkD"},  // Count: 1, Ordinal: 3195
            {"N'SELECT DISTINCT DESCRIPTION FROM CSCAUSES WHERE CAUSEID = '", "LkD"},  // Count: 1, Ordinal: 3196
            {"N'SELECT DISTINCT INSTRUCTIONS FROM CSACTIONINSTRUCTIONS WHERE CAUSEID = '", "MkD"},  // Count: 1, Ordinal: 3197
            {"N'SELECT DISTINCT INSTRUCTIONS FROM CSCAUSEINSTRUCTIONS WHERE STRATEGYID = '", "AlD"},  // Count: 1, Ordinal: 3198
            {"N'SELECT DISTINCT RULES . AREA ", "BlD"},  // Count: 1, Ordinal: 3199
            {"N'SELECT DISTINCT STRATEGYID ", "ClD"},  // Count: 1, Ordinal: 3200
            {"N'SELECT M . RULEID ", "DlD"},  // Count: 1, Ordinal: 3201
            {"N'SET ANSI_WARNINGS ON ; SET ANSI_NULLS ON ; DELETE FROM '", "ElD"},  // Count: 1, Ordinal: 3202
            {"N'SQL TRACE ALREADY EXISTS'", "FlD"},  // Count: 1, Ordinal: 3203
            {"N'SQL TRACE IS NOT CREATED'", "GlD"},  // Count: 1, Ordinal: 3204
            {"N'STILL OK'", "HlD"},  // Count: 1, Ordinal: 3205
            {"N'''TAG'''", "IlD"},  // Count: 1, Ordinal: 3206
            {"N'THE DATABASE DOES NOT EXIST'", "JlD"},  // Count: 1, Ordinal: 3207
            {"N'THE PRODUNIT CHECK'", "KlD"},  // Count: 1, Ordinal: 3208
            {"N'THE VALUE OF THE EXLUDEPROGRAM IS NULL'", "LlD"},  // Count: 1, Ordinal: 3209
            {"N'TRACEFILEPATH'", "MlD"},  // Count: 1, Ordinal: 3210
            {"N'TYPE '", "AmD"},  // Count: 1, Ordinal: 3211
            {"N'UPDATE '", "BmD"},  // Count: 1, Ordinal: 3212
            {"N'UPDATE CSMAILBOXES SET STATUSID = '", "CmD"},  // Count: 1, Ordinal: 3213
            {"N'UPDATED'", "DmD"},  // Count: 1, Ordinal: 3214
            {"N'WHERE MAILBOXID = '", "EmD"},  // Count: 1, Ordinal: 3215
            {"N'Y_BT_CL'", "FmD"},  // Count: 1, Ordinal: 3216
            {"N'YBSTAT'", "GmD"},  // Count: 1, Ordinal: 3217
            {"N'YCPYQ'", "HmD"},  // Count: 1, Ordinal: 3218
            {"N'YGMNCHG'", "ImD"},  // Count: 1, Ordinal: 3219
            {"N'YHB'", "JmD"},  // Count: 1, Ordinal: 3220
            {"N'YPHCON'", "KmD"},  // Count: 1, Ordinal: 3221
            {"N'YPKB'", "LmD"},  // Count: 1, Ordinal: 3222
            {"N'YPOREC'", "MmD"},  // Count: 1, Ordinal: 3223
            {"N'YPRD_NPO'", "AnD"},  // Count: 1, Ordinal: 3224
            {"N'YPROD'", "BnD"},  // Count: 1, Ordinal: 3225
            {"N'YPRODADJ'", "CnD"},  // Count: 1, Ordinal: 3226
            {"N'YPRU'", "DnD"},  // Count: 1, Ordinal: 3227
            {"N'YQA01'", "EnD"},  // Count: 1, Ordinal: 3228
            {"N'YRC_STAT'", "FnD"},  // Count: 1, Ordinal: 3229
            {"N'YREWORK'", "GnD"},  // Count: 1, Ordinal: 3230
            {"N'YUPT'", "HnD"},  // Count: 1, Ordinal: 3231
            {"N'YXFER911'", "InD"},  // Count: 1, Ordinal: 3232
            {"ON_ERROR1", "JnD"},  // Count: 1, Ordinal: 3233
            {"ON_ERROR2", "KnD"},  // Count: 1, Ordinal: 3234
            {"ON_ERROR3", "LnD"},  // Count: 1, Ordinal: 3235
            {"ON_ERROR4", "MnD"},  // Count: 1, Ordinal: 3236
            {"ON_ERROR5:", "AoD"},  // Count: 1, Ordinal: 3237
            {"ON_ERROR6:", "BoD"},  // Count: 1, Ordinal: 3238
            {"ON_ERROR7:", "CoD"},  // Count: 1, Ordinal: 3239
            {"ON_EXIT:", "DoD"},  // Count: 1, Ordinal: 3240
            {"OPERATORID'", "EoD"},  // Count: 1, Ordinal: 3241
            {"OPTIONS", "FoD"},  // Count: 1, Ordinal: 3242
            {"ORDERNUMBER", "GoD"},  // Count: 1, Ordinal: 3243
            {"PACKAGE_PARTS", "HoD"},  // Count: 1, Ordinal: 3244
            {"PC", "IoD"},  // Count: 1, Ordinal: 3245
            {"PC_NAME", "JoD"},  // Count: 1, Ordinal: 3246
            {"PCBO_AREA_RESOURCE", "KoD"},  // Count: 1, Ordinal: 3247
            {"PCBO_EMAIL_FOR_ERRORS", "LoD"},  // Count: 1, Ordinal: 3248
            {"PCBO_GMN_NOBATCH_VALIDATION", "MoD"},  // Count: 1, Ordinal: 3249
            {"PCBO_GMN_TO_STOR_LOCATION", "ApD"},  // Count: 1, Ordinal: 3250
            {"PCBO_PD_TAGS", "BpD"},  // Count: 1, Ordinal: 3251
            {"PCBO_PM_GRADE", "CpD"},  // Count: 1, Ordinal: 3252
            {"PCBO_PROCESS_MESSAGE", "DpD"},  // Count: 1, Ordinal: 3253
            {"PCBO_RESOURCE_DCSBATCHMODULE", "EpD"},  // Count: 1, Ordinal: 3254
            {"PCBO_RESOURCE_LIST", "FpD"},  // Count: 1, Ordinal: 3255
            {"PCBO_SAP_AVAILABILITY", "GpD"},  // Count: 1, Ordinal: 3256
            {"PCBO_TRANSACTION_LOG", "HpD"},  // Count: 1, Ordinal: 3257
            {"PCPROCESSCHANGEHISTORY", "IpD"},  // Count: 1, Ordinal: 3258
            {"PHASE_ID", "JpD"},  // Count: 1, Ordinal: 3259
            {"PI_TAG", "KpD"},  // Count: 1, Ordinal: 3260
            {"PIMS_SP_CHART_GETMAX_MASK_LEVEL_FROM_CHARTLEVELDATA", "LpD"},  // Count: 1, Ordinal: 3261
            {"PIMS_SP_CHART_INSERT_CHARTLEVELDATA", "MpD"},  // Count: 1, Ordinal: 3262
            {"PIMS_SP_CHART_SELECT_BY_NUMBER_FROM_CHARTLEVELDATA", "AqD"},  // Count: 1, Ordinal: 3263
            {"PIMS_SP_CHART_SELECT_BY_PATH_FROM_CHARTLEVELDATA", "BqD"},  // Count: 1, Ordinal: 3264
            {"PIMS_SP_CHART_SELECT_CHARTMENULEVELS", "CqD"},  // Count: 1, Ordinal: 3265
            {"PIMS_SP_CHART_SELECT_FROM_CHARTLEVELDATA", "DqD"},  // Count: 1, Ordinal: 3266
            {"PIMS_SP_CHART_SELECT_FULLPATH_FROM_CHARTLEVELDATA", "EqD"},  // Count: 1, Ordinal: 3267
            {"PIMS_SP_CHART_SELECT_MASK_LEVEL_DATA_FROM_CHARTLEVELDATA", "FqD"},  // Count: 1, Ordinal: 3268
            {"PIMS_SP_CHART_SELECT_PARENT_BATCH_DATA_FROM_CHARTLEVELDATA", "GqD"},  // Count: 1, Ordinal: 3269
            {"PIMS_SP_CHART_UPDATE_BRANCH_CHARTLEVELDATA", "HqD"},  // Count: 1, Ordinal: 3270
            {"PIMS_SP_CHART_UPDATE_CHARTLEVELDATA", "IqD"},  // Count: 1, Ordinal: 3271
            {"PIMS_SP_CHART_UPDATE_CHARTLEVELDATA_INCLPATH", "JqD"},  // Count: 1, Ordinal: 3272
            {"PIMS_SP_CHARTLEVELDATASTATUS", "KqD"},  // Count: 1, Ordinal: 3273
            {"PIMS_SP_CHARTMAINT_DELETE_ALL_BATCHTYPEEXPRESSIONS", "LqD"},  // Count: 1, Ordinal: 3274
            {"PIMS_SP_CHARTMAINT_DELETE_CHARTLEVELDATA", "MqD"},  // Count: 1, Ordinal: 3275
            {"PIMS_SP_CHARTMAINT_DELETE_CHARTMENULEVELS", "ArD"},  // Count: 1, Ordinal: 3276
            {"PIMS_SP_CHARTMAINT_DELETE_DEFAULTTAGMASK", "BrD"},  // Count: 1, Ordinal: 3277
            {"PIMS_SP_CHARTMAINT_DELETE_GLOBALBATCHLINK", "CrD"},  // Count: 1, Ordinal: 3278
            {"PIMS_SP_CHARTMAINT_DELETE_GLOBALFILTERLINK", "DrD"},  // Count: 1, Ordinal: 3279
            {"PIMS_SP_CHARTMAINT_INSERT_BATCHTYPEEXPRESSION", "ErD"},  // Count: 1, Ordinal: 3280
            {"PIMS_SP_CHARTMAINT_INSERT_CHARTMENULEVELS", "FrD"},  // Count: 1, Ordinal: 3281
            {"PIMS_SP_CHARTMAINT_SELECT_ALL_GLOBALFILTERLINK", "GrD"},  // Count: 1, Ordinal: 3282
            {"PIMS_SP_CHARTMAINT_SELECT_ALL_MASK", "HrD"},  // Count: 1, Ordinal: 3283
            {"PIMS_SP_CHARTMAINT_SELECT_BATCHTYPEEXPRESSIONS", "IrD"},  // Count: 1, Ordinal: 3284
            {"PIMS_SP_CHARTMAINT_SELECT_CHARTMENULEVELS_BYNUMBER", "JrD"},  // Count: 1, Ordinal: 3285
            {"PIMS_SP_CHARTMAINT_SELECT_DEFAULTTAGMASK", "KrD"},  // Count: 1, Ordinal: 3286
            {"PIMS_SP_CHARTMAINT_SELECT_GLOBALBATCHLINK", "LrD"},  // Count: 1, Ordinal: 3287
            {"PIMS_SP_CHARTMAINT_UPDATE_CHARTLEVELDATA", "MrD"},  // Count: 1, Ordinal: 3288
            {"PIMS_SP_CHARTMAINT_UPDATE_CHARTMENULEVELS", "AsD"},  // Count: 1, Ordinal: 3289
            {"PIMS_SP_CHARTMAINT_UPDATE_DEFAULTTAGMASK", "BsD"},  // Count: 1, Ordinal: 3290
            {"PIMS_SP_CHARTMAINT_UPDATE_GLOBALBATCHLINK", "CsD"},  // Count: 1, Ordinal: 3291
            {"PIMS_SP_CHARTMAINT_UPDATE_GLOBALFILTERLINK", "DsD"},  // Count: 1, Ordinal: 3292
            {"PIMS_SP_DSPCCALCULATIONSET_CHANGEDATEOFLASTCALC", "EsD"},  // Count: 1, Ordinal: 3293
            {"PIMS_SP_DSPCCALCULATIONSET_DELETE", "FsD"},  // Count: 1, Ordinal: 3294
            {"PIMS_SP_DSPCCALCULATIONSET_INSERT", "GsD"},  // Count: 1, Ordinal: 3295
            {"PIMS_SP_DSPCCALCULATIONSET_SELECT_BY_NAME", "HsD"},  // Count: 1, Ordinal: 3296
            {"PIMS_SP_DSPCCALCULATIONSET_UPDATE", "IsD"},  // Count: 1, Ordinal: 3297
            {"PIMS_SP_DSPCCALCULATIONSETTAGS_DELETE", "JsD"},  // Count: 1, Ordinal: 3298
            {"PIMS_SP_DSPCCALCULATIONSETTAGS_INSERT", "KsD"},  // Count: 1, Ordinal: 3299
            {"PIMS_SP_DSPCCALCULATIONSETTAGS_UPDATE", "LsD"},  // Count: 1, Ordinal: 3300
            {"PIMS_SP_DSPCGROUPS_DELETE", "MsD"},  // Count: 1, Ordinal: 3301
            {"PIMS_SP_DSPCGROUPS_INSERT", "AtD"},  // Count: 1, Ordinal: 3302
            {"PIMS_SP_DSPCGROUPS_UPDATE", "BtD"},  // Count: 1, Ordinal: 3303
            {"PIMS_SP_DSPCTAGCHANGEHISTORY_DELETE", "CtD"},  // Count: 1, Ordinal: 3304
            {"PIMS_SP_DSPCTAGCHANGEHISTORY_INSERT", "DtD"},  // Count: 1, Ordinal: 3305
            {"PIMS_SP_DSPCTAGCHANGEHISTORY_UPDATE", "EtD"},  // Count: 1, Ordinal: 3306
            {"PIMS_SP_GETCALCSETGROUPS", "FtD"},  // Count: 1, Ordinal: 3307
            {"PIMS_SP_GETCALCSETSALL", "GtD"},  // Count: 1, Ordinal: 3308
            {"PIMS_SP_GETCALCSETSBYGROUPSET", "HtD"},  // Count: 1, Ordinal: 3309
            {"PIMS_SP_GETPIMSGROUPS", "ItD"},  // Count: 1, Ordinal: 3310
            {"PIMS_SP_GETPIMSNTGROUPS", "JtD"},  // Count: 1, Ordinal: 3311
            {"PIMS_SP_GETPIMSROLES", "KtD"},  // Count: 1, Ordinal: 3312
            {"PIMS_SP_GETPIMSUSERINFO", "LtD"},  // Count: 1, Ordinal: 3313
            {"PIMS_SP_INSERT_YINVADJ_MESSAGELOG", "MtD"},  // Count: 1, Ordinal: 3314
            {"PIMS_SP_INSERT_YINVADJ_PIINVENTORY", "AuD"},  // Count: 1, Ordinal: 3315
            {"PIMS_SP_INSERT_YINVADJ_SUMMARIZEDPIINVENTORY", "BuD"},  // Count: 1, Ordinal: 3316
            {"PIMS_SP_PIMSACCOUNTING_INSERT", "CuD"},  // Count: 1, Ordinal: 3317
            {"PIMS_SP_PIMSACCOUNTING_INSERT_2", "DuD"},  // Count: 1, Ordinal: 3318
            {"PIMS_SP_PIMSACCOUNTING_PURGE", "EuD"},  // Count: 1, Ordinal: 3319
            {"PIMS_SP_PIMSAPPLICATIONS_DELETE", "FuD"},  // Count: 1, Ordinal: 3320
            {"PIMS_SP_PIMSAPPLICATIONS_INSERT", "GuD"},  // Count: 1, Ordinal: 3321
            {"PIMS_SP_PIMSAPPLICATIONS_SELECT", "HuD"},  // Count: 1, Ordinal: 3322
            {"PIMS_SP_PIMSAPPLICATIONS_UPDATE", "IuD"},  // Count: 1, Ordinal: 3323
            {"PIMS_SP_PIMSENVIRONMENT_DELETE", "JuD"},  // Count: 1, Ordinal: 3324
            {"PIMS_SP_PIMSENVIRONMENT_INSERT", "KuD"},  // Count: 1, Ordinal: 3325
            {"PIMS_SP_PIMSENVIRONMENT_SELECT", "LuD"},  // Count: 1, Ordinal: 3326
            {"PIMS_SP_PIMSENVIRONMENT_UPDATE", "MuD"},  // Count: 1, Ordinal: 3327
            {"PIMS_SP_PIMSENVIRONMENTCHANGEHISTORY_INSERT", "AvD"},  // Count: 1, Ordinal: 3328
            {"PIMS_SP_PIMSENVIRONMENTCHANGEHISTORY_SELECT", "BvD"},  // Count: 1, Ordinal: 3329
            {"PIMS_SP_PIMSENVIRONMENTNAMEDVALUE_DELETE", "CvD"},  // Count: 1, Ordinal: 3330
            {"PIMS_SP_PIMSENVIRONMENTNAMEDVALUE_INSERT", "DvD"},  // Count: 1, Ordinal: 3331
            {"PIMS_SP_PIMSENVIRONMENTNAMEDVALUE_SELECT", "EvD"},  // Count: 1, Ordinal: 3332
            {"PIMS_SP_PIMSENVIRONMENTNAMEDVALUE_UPDATE", "FvD"},  // Count: 1, Ordinal: 3333
            {"PIMS_SP_PIMSENVIRONMENTNAMEDVALUECHANGEHISTORY_INSERT", "GvD"},  // Count: 1, Ordinal: 3334
            {"PIMS_SP_PIMSENVIRONMENTNAMEDVALUECHANGEHISTORY_SELECT", "HvD"},  // Count: 1, Ordinal: 3335
            {"PIMS_SP_PIMSENVIRONMENTNAMEDVALUECHANGEHISTORY_SELECTALL", "IvD"},  // Count: 1, Ordinal: 3336
            {"PIMS_SP_PIMSGROUPS_DELETE", "JvD"},  // Count: 1, Ordinal: 3337
            {"PIMS_SP_PIMSGROUPS_INSERT", "KvD"},  // Count: 1, Ordinal: 3338
            {"PIMS_SP_PIMSGROUPS_UPDATE", "LvD"},  // Count: 1, Ordinal: 3339
            {"PIMS_SP_PIMSPISERVERS_SELECT", "MvD"},  // Count: 1, Ordinal: 3340
            {"PIMS_SP_PIMSROLEAPPPRIVS_DELETE", "AwD"},  // Count: 1, Ordinal: 3341
            {"PIMS_SP_PIMSROLEAPPPRIVS_INSERT", "BwD"},  // Count: 1, Ordinal: 3342
            {"PIMS_SP_PIMSROLEAPPPRIVS_SELECT_BYROLENAME", "CwD"},  // Count: 1, Ordinal: 3343
            {"PIMS_SP_PIMSROLEAPPPRIVS_UPDATE", "DwD"},  // Count: 1, Ordinal: 3344
            {"PIMS_SP_PIMSROLEGROUPPRIVS_DELETE", "EwD"},  // Count: 1, Ordinal: 3345
            {"PIMS_SP_PIMSROLEGROUPPRIVS_INSERT", "FwD"},  // Count: 1, Ordinal: 3346
            {"PIMS_SP_PIMSROLEGROUPPRIVS_SELECT_BYROLENAME", "GwD"},  // Count: 1, Ordinal: 3347
            {"PIMS_SP_PIMSROLEGROUPPRIVS_UPDATE", "HwD"},  // Count: 1, Ordinal: 3348
            {"PIMS_SP_PIMSROLES_DELETE", "IwD"},  // Count: 1, Ordinal: 3349
            {"PIMS_SP_PIMSROLES_INSERT", "JwD"},  // Count: 1, Ordinal: 3350
            {"PIMS_SP_PIMSROLES_SELECT_BYROLENAME", "KwD"},  // Count: 1, Ordinal: 3351
            {"PIMS_SP_PIMSROLES_UPDATE", "LwD"},  // Count: 1, Ordinal: 3352
            {"PIMS_SP_PIMSUSERROLES_DELETE", "MwD"},  // Count: 1, Ordinal: 3353
            {"PIMS_SP_PIMSUSERROLES_INSERT", "AxD"},  // Count: 1, Ordinal: 3354
            {"PIMS_SP_PIMSUSERROLES_UPDATE", "BxD"},  // Count: 1, Ordinal: 3355
            {"PIMS_SP_PIMSUSERS_DELETE", "CxD"},  // Count: 1, Ordinal: 3356
            {"PIMS_SP_PIMSUSERS_INSERT", "DxD"},  // Count: 1, Ordinal: 3357
            {"PIMS_SP_PIMSUSERS_SELECT", "ExD"},  // Count: 1, Ordinal: 3358
            {"PIMS_SP_PIMSUSERS_UPDATE", "FxD"},  // Count: 1, Ordinal: 3359
            {"PIMS_SP_SELECT_ALL_FROM_YINVADJ_INVENTORYPITAGS", "GxD"},  // Count: 1, Ordinal: 3360
            {"PIMS_SP_SELECT_ALL_FROM_YINVADJ_PROCMSGDATA", "HxD"},  // Count: 1, Ordinal: 3361
            {"PIMS_SP_SELECT_BY_GMN_AND_RUNDATE_FROM_YINVADJ_PIINVENTORY", "IxD"},  // Count: 1, Ordinal: 3362
            {"PIMS_SP_UPDATEMASKLEVELSTATUS", "JxD"},  // Count: 1, Ordinal: 3363
            {"PIMSARCHIVESHIPPEDBATCHES", "KxD"},  // Count: 1, Ordinal: 3364
            {"PIMSBACKUPRLINKTRANSACTIONS", "LxD"},  // Count: 1, Ordinal: 3365
            {"PIMSDATE", "MxD"},  // Count: 1, Ordinal: 3366
            {"PIMSDELETESHIPPEDBATCHES", "AyD"},  // Count: 1, Ordinal: 3367
            {"PIMSGRADE", "ByD"},  // Count: 1, Ordinal: 3368
            {"PIMSINSERTSHIPPEDBATCHES", "CyD"},  // Count: 1, Ordinal: 3369
            {"PIMSNAMEDVALUEVALUE", "DyD"},  // Count: 1, Ordinal: 3370
            {"PIMSPISERVERS", "EyD"},  // Count: 1, Ordinal: 3371
            {"PIMSPLANT_ID", "FyD"},  // Count: 1, Ordinal: 3372
            {"PIMSPM", "GyD"},  // Count: 1, Ordinal: 3373
            {"PIMSPROC_MESSAGE", "HyD"},  // Count: 1, Ordinal: 3374
            {"PIMSPURGEPENDINGRLINKTRANACTIONS", "IyD"},  // Count: 1, Ordinal: 3375
            {"PIMSPURGEPENDINGRLINKTRANSACTIONS", "JyD"},  // Count: 1, Ordinal: 3376
            {"PIMSSAP_LOGON", "KyD"},  // Count: 1, Ordinal: 3377
            {"PIMSSDKRESPONSETIME", "LyD"},  // Count: 1, Ordinal: 3378
            {"PIMSUSER", "MyD"},  // Count: 1, Ordinal: 3379
            {"PKG_COUNT", "AzE"},  // Count: 1, Ordinal: 3380
            {"PKG_SIZE", "BzE"},  // Count: 1, Ordinal: 3381
            {"PMIS_SP_SELECT_ALL_FROM_YINVADJ_INVENTORYPITAGS", "CzE"},  // Count: 1, Ordinal: 3382
            {"PMIS_SP_SELECT_ALL_FROM_YINVADJ_PROCMSGDATA", "DzE"},  // Count: 1, Ordinal: 3383
            {"PMIS_SP_SELECT_BY_GMN_AND_RUNDATE_FROM_YINVADJ_PIINVENTORY", "EzE"},  // Count: 1, Ordinal: 3384
            {"POINTID", "FzE"},  // Count: 1, Ordinal: 3385
            {"PRINT_QUEUE", "GzE"},  // Count: 1, Ordinal: 3386
            {"PROCESS_CHANGE_DATETIME", "HzE"},  // Count: 1, Ordinal: 3387
            {"PROD", "IzE"},  // Count: 1, Ordinal: 3388
            {"QM_DELETE_QMDATA", "JzE"},  // Count: 1, Ordinal: 3389
            {"QM_DELETE_QMDEPARTMENTS", "KzE"},  // Count: 1, Ordinal: 3390
            {"QM_DELETE_QMPOINTS", "LzE"},  // Count: 1, Ordinal: 3391
            {"QM_GETADEPARTMENT", "MzE"},  // Count: 1, Ordinal: 3392
            {"QM_GETALLDATA_DATE", "AAE"},  // Count: 1, Ordinal: 3393
            {"QM_GETALLDEPARTMENTS", "BAE"},  // Count: 1, Ordinal: 3394
            {"QM_GETALLQMDATA", "CAE"},  // Count: 1, Ordinal: 3395
            {"QM_GETALLQMPOINTS", "DAE"},  // Count: 1, Ordinal: 3396
            {"QM_GETAQMID", "EAE"},  // Count: 1, Ordinal: 3397
            {"QM_GETDATABYQMID", "FAE"},  // Count: 1, Ordinal: 3398
            {"QM_GETDATABYQMID_DATE", "GAE"},  // Count: 1, Ordinal: 3399
            {"QM_GETPOINTS_DEPT", "HAE"},  // Count: 1, Ordinal: 3400
            {"QM_GETPOINTS_DESC", "IAE"},  // Count: 1, Ordinal: 3401
            {"QM_GETPOINTS_GROUP", "JAE"},  // Count: 1, Ordinal: 3402
            {"QM_GETPOINTS_PRODUNIT", "KAE"},  // Count: 1, Ordinal: 3403
            {"QM_INSERT_QMDATA", "LAE"},  // Count: 1, Ordinal: 3404
            {"QM_INSERT_QMDEPARTMENTS", "MAE"},  // Count: 1, Ordinal: 3405
            {"QM_INSERT_QMPOINTS", "ABE"},  // Count: 1, Ordinal: 3406
            {"QM_UPDATE_QMDATA", "BBE"},  // Count: 1, Ordinal: 3407
            {"QM_UPDATE_QMDATA_PART", "CBE"},  // Count: 1, Ordinal: 3408
            {"QM_UPDATE_QMDEPARTMENTS", "DBE"},  // Count: 1, Ordinal: 3409
            {"QM_UPDATE_QMPOINTS", "EBE"},  // Count: 1, Ordinal: 3410
            {"QTYSHIPPEDGROSS", "FBE"},  // Count: 1, Ordinal: 3411
            {"QTYSHIPPEDNET", "GBE"},  // Count: 1, Ordinal: 3412
            {"QTYSHIPPEDUOM", "HBE"},  // Count: 1, Ordinal: 3413
            {"READINGXML", "IBE"},  // Count: 1, Ordinal: 3414
            {"RECORDNUMBER", "JBE"},  // Count: 1, Ordinal: 3415
            {"RESOURCE", "KBE"},  // Count: 1, Ordinal: 3416
            {"RULES'", "LBE"},  // Count: 1, Ordinal: 3417
            {"SAF_INSERT_CHART", "MBE"},  // Count: 1, Ordinal: 3418
            {"SAF_INSERT_LIMS", "ACE"},  // Count: 1, Ordinal: 3419
            {"SAF_INSERT_PIMS", "BCE"},  // Count: 1, Ordinal: 3420
            {"SAF_INSERT_QUERY", "CCE"},  // Count: 1, Ordinal: 3421
            {"SAP_AVAILABLE", "DCE"},  // Count: 1, Ordinal: 3422
            {"SCHEDULEDSHIPDATE", "ECE"},  // Count: 1, Ordinal: 3423
            {"SEC_DELETE_PIMSADMIN", "FCE"},  // Count: 1, Ordinal: 3424
            {"SEC_DELETE_PIMSADMINSUBLEVEL", "GCE"},  // Count: 1, Ordinal: 3425
            {"SEC_INSERT_PIMSADMIN", "HCE"},  // Count: 1, Ordinal: 3426
            {"SEC_INSERT_PIMSADMINSUBLEVEL", "ICE"},  // Count: 1, Ordinal: 3427
            {"SEC_INSERT_PIMSCHANGEHISTORY", "JCE"},  // Count: 1, Ordinal: 3428
            {"SEC_SELECT_APPLICATIONS", "KCE"},  // Count: 1, Ordinal: 3429
            {"SEC_SELECT_APPNTGROUPS", "LCE"},  // Count: 1, Ordinal: 3430
            {"SEC_SELECT_APPSUBLEVELNTGROUPS", "MCE"},  // Count: 1, Ordinal: 3431
            {"SEC_SELECT_APPSUBLEVELNTGROUPS_CHAR", "ADE"},  // Count: 1, Ordinal: 3432
            {"SEC_SELECT_ORPHANSUBLEVEL_CSAREA", "BDE"},  // Count: 1, Ordinal: 3433
            {"SEC_SELECT_ORPHANSUBLEVEL_GROUP", "CDE"},  // Count: 1, Ordinal: 3434
            {"SEC_SELECT_ORPHANSUBLEVEL_RDMGROUP", "DDE"},  // Count: 1, Ordinal: 3435
            {"SEC_SELECT_PIMSADMINSUPPORTGROUPS", "EDE"},  // Count: 1, Ordinal: 3436
            {"SEC_SELECT_PIMSAPPID", "FDE"},  // Count: 1, Ordinal: 3437
            {"SECONDARYINFO", "GDE"},  // Count: 1, Ordinal: 3438
            {"SECPIMSADMINSUPPORTGROUPS", "HDE"},  // Count: 1, Ordinal: 3439
            {"'SELECT DISTINCT CITATIONS . CITATIONID ", "IDE"},  // Count: 1, Ordinal: 3440
            {"'SETUSER'", "JDE"},  // Count: 1, Ordinal: 3441
            {"SHIPPINGPOINT", "KDE"},  // Count: 1, Ordinal: 3442
            {"SHIPTOCITY", "LDE"},  // Count: 1, Ordinal: 3443
            {"SMALLDATETIME", "MDE"},  // Count: 1, Ordinal: 3444
            {"SMTESERVERADMINS", "AEE"},  // Count: 1, Ordinal: 3445
            {"SOLDTOCITY", "BEE"},  // Count: 1, Ordinal: 3446
            {"SORT'", "CEE"},  // Count: 1, Ordinal: 3447
            {"SP_ARCHIVECITATIONTABLES", "DEE"},  // Count: 1, Ordinal: 3448
            {"SP_CHECK_SORT_NULL", "EEE"},  // Count: 1, Ordinal: 3449
            {"SP_DELETE_APPLICATION_1", "FEE"},  // Count: 1, Ordinal: 3450
            {"SP_DELETE_CAUSEACTION", "GEE"},  // Count: 1, Ordinal: 3451
            {"SP_DELETE_CS_TO_PI_1", "HEE"},  // Count: 1, Ordinal: 3452
            {"SP_DELETE_CS_TO_PIGROUP_1", "IEE"},  // Count: 1, Ordinal: 3453
            {"SP_DELETE_EQUIPMENT_1", "JEE"},  // Count: 1, Ordinal: 3454
            {"SP_DELETE_GROUP_1", "KEE"},  // Count: 1, Ordinal: 3455
            {"SP_DELETE_GROUPLISTAREALISTSANDTAGS", "LEE"},  // Count: 1, Ordinal: 3456
            {"SP_DELETE_LIMITS_GROUP_1", "MEE"},  // Count: 1, Ordinal: 3457
            {"SP_DELETE_LIMS_INFORMATION_BY_UNIT_NEW", "AFE"},  // Count: 1, Ordinal: 3458
            {"SP_DELETE_LIST_1", "BFE"},  // Count: 1, Ordinal: 3459
            {"SP_DELETE_LIST_AREA_1", "CFE"},  // Count: 1, Ordinal: 3460
            {"SP_DELETE_LIST_POINTS_1", "DFE"},  // Count: 1, Ordinal: 3461
            {"SP_DELETE_LISTANDPOINTS", "EFE"},  // Count: 1, Ordinal: 3462
            {"SP_DELETE_LISTANDTAGS_1", "FFE"},  // Count: 1, Ordinal: 3463
            {"SP_DELETE_LISTAREAANDLISTS_1", "GFE"},  // Count: 1, Ordinal: 3464
            {"SP_DELETE_LISTAREALISTSANDTAGS_1", "HFE"},  // Count: 1, Ordinal: 3465
            {"SP_DELETE_LISTAREALISTSANDTAGSBYGROUP_1", "IFE"},  // Count: 1, Ordinal: 3466
            {"SP_DELETE_LISTAREALISTSTAGSANDGROUP_1", "JFE"},  // Count: 1, Ordinal: 3467
            {"SP_DELETE_ORDERED_LIST_1", "KFE"},  // Count: 1, Ordinal: 3468
            {"SP_DELETE_ORDERED_LIST_LIST_1", "LFE"},  // Count: 1, Ordinal: 3469
            {"SP_DELETE_PI_1", "MFE"},  // Count: 1, Ordinal: 3470
            {"SP_DELETE_PRINTING_1", "AGE"},  // Count: 1, Ordinal: 3471
            {"SP_DELETE_PROCESS_LIMITS_1", "BGE"},  // Count: 1, Ordinal: 3472
            {"SP_DELETE_PROCESS_LIMITS_BY_GROUP", "CGE"},  // Count: 1, Ordinal: 3473
            {"SP_DELETE_PROCESS_LIMITS_BY_UNIT", "DGE"},  // Count: 1, Ordinal: 3474
            {"SP_DELETE_PRODUCTION_UNIT_BY_GROUP", "EGE"},  // Count: 1, Ordinal: 3475
            {"SP_DELETE_RDMALARMHISTORY_BY_RDMALARMDATE", "FGE"},  // Count: 1, Ordinal: 3476
            {"SP_DELETE_RDMGROUP", "GGE"},  // Count: 1, Ordinal: 3477
            {"SP_DELETE_RDMSERVER", "HGE"},  // Count: 1, Ordinal: 3478
            {"SP_DELETE_RDMTAGS", "IGE"},  // Count: 1, Ordinal: 3479
            {"SP_DELETE_SERVERGROUPTAGS_BY_SERVER", "JGE"},  // Count: 1, Ordinal: 3480
            {"SP_DELETE_SORT", "KGE"},  // Count: 1, Ordinal: 3481
            {"SP_DELETE_SPC_LIMITS_1", "LGE"},  // Count: 1, Ordinal: 3482
            {"SP_DELETE_TAGS_1", "MGE"},  // Count: 1, Ordinal: 3483
            {"SP_DELETE_TEMP", "AHE"},  // Count: 1, Ordinal: 3484
            {"SP_DELETE_TYPES", "BHE"},  // Count: 1, Ordinal: 3485
            {"SP_DERIVESQCSTATUS", "CHE"},  // Count: 1, Ordinal: 3486
            {"SP_EXECUTESQL", "DHE"},  // Count: 1, Ordinal: 3487
            {"SP_FILTER_CITATIONS", "EHE"},  // Count: 1, Ordinal: 3488
            {"SP_INCREMENT_NOOFREMINDERMAILS_BY_QUARTER_YEAR", "FHE"},  // Count: 1, Ordinal: 3489
            {"SP_INSERT_ACCESS", "GHE"},  // Count: 1, Ordinal: 3490
            {"SP_INSERT_APPLICATION_1", "HHE"},  // Count: 1, Ordinal: 3491
            {"SP_INSERT_CAUSEACTION", "IHE"},  // Count: 1, Ordinal: 3492
            {"SP_INSERT_CS_TO_PI_1", "JHE"},  // Count: 1, Ordinal: 3493
            {"SP_INSERT_CS_TO_PIGROUP_1", "KHE"},  // Count: 1, Ordinal: 3494
            {"SP_INSERT_EQUIPMENT_1", "LHE"},  // Count: 1, Ordinal: 3495
            {"SP_INSERT_GROUP", "MHE"},  // Count: 1, Ordinal: 3496
            {"SP_INSERT_GROUP_1", "AIE"},  // Count: 1, Ordinal: 3497
            {"SP_INSERT_LIMITS_GROUP_1", "BIE"},  // Count: 1, Ordinal: 3498
            {"SP_INSERT_LIST", "CIE"},  // Count: 1, Ordinal: 3499
            {"SP_INSERT_LIST_1", "DIE"},  // Count: 1, Ordinal: 3500
            {"SP_INSERT_LIST_AREA", "EIE"},  // Count: 1, Ordinal: 3501
            {"SP_INSERT_LIST_AREA_1", "FIE"},  // Count: 1, Ordinal: 3502
            {"SP_INSERT_LIST_POINTS_1", "GIE"},  // Count: 1, Ordinal: 3503
            {"SP_INSERT_ORDERED_LIST_1", "HIE"},  // Count: 1, Ordinal: 3504
            {"SP_INSERT_ORDERED_LIST_LIST_1", "IIE"},  // Count: 1, Ordinal: 3505
            {"SP_INSERT_PI_1", "JIE"},  // Count: 1, Ordinal: 3506
            {"SP_INSERT_PIMSCONTACTACTION", "KIE"},  // Count: 1, Ordinal: 3507
            {"SP_INSERT_PRINTING_1", "LIE"},  // Count: 1, Ordinal: 3508
            {"SP_INSERT_PROCESS_LIMITS_1", "MIE"},  // Count: 1, Ordinal: 3509
            {"SP_INSERT_PROCESS_LIMITS_NEW", "AJE"},  // Count: 1, Ordinal: 3510
            {"SP_INSERT_PRODUCTION_UNIT_1", "BJE"},  // Count: 1, Ordinal: 3511
            {"SP_INSERT_PRODUCTION_UNIT_WCOMMENT", "CJE"},  // Count: 1, Ordinal: 3512
            {"SP_INSERT_PRODUCTION_UNIT_WCOMMENTXX", "DJE"},  // Count: 1, Ordinal: 3513
            {"SP_INSERT_RDMALARMHISTORY", "EJE"},  // Count: 1, Ordinal: 3514
            {"SP_INSERT_RDMGROUP", "FJE"},  // Count: 1, Ordinal: 3515
            {"SP_INSERT_RDMSERVER", "GJE"},  // Count: 1, Ordinal: 3516
            {"SP_INSERT_RDMTAGS", "HJE"},  // Count: 1, Ordinal: 3517
            {"SP_INSERT_RDMTAGS_2", "IJE"},  // Count: 1, Ordinal: 3518
            {"SP_INSERT_TYPES", "JJE"},  // Count: 1, Ordinal: 3519
            {"SP_INSERTTAGSRECORDS", "KJE"},  // Count: 1, Ordinal: 3520
            {"SP_MOVEACTION", "LJE"},  // Count: 1, Ordinal: 3521
            {"SP_MOVECAUSE", "MJE"},  // Count: 1, Ordinal: 3522
            {"SP_MSDEL_DENNIS", "AKE"},  // Count: 1, Ordinal: 3523
            {"SP_MSINS_DENNIS", "BKE"},  // Count: 1, Ordinal: 3524
            {"SP_MSUPD_DENNIS", "CKE"},  // Count: 1, Ordinal: 3525
            {"SP_PROCCHG_DELETE_LINK_BY_BASETAG_LINKEDTAG", "DKE"},  // Count: 1, Ordinal: 3526
            {"SP_PROCCHG_DELETE_PROCESS_LIMITS_BY_GROUP_PRODUCT_PROCESS_TAG", "EKE"},  // Count: 1, Ordinal: 3527
            {"SP_PROCCHG_DELETE_PROCESS_LIMITS_BY_GROUP_UNIT_TAG", "FKE"},  // Count: 1, Ordinal: 3528
            {"SP_PROCCHG_DELETE_PROCESS_LIMITS_BY_GROUP_UNIT_TAG1", "GKE"},  // Count: 1, Ordinal: 3529
            {"SP_PROCCHG_INSERT_BASE_UNIT", "HKE"},  // Count: 1, Ordinal: 3530
            {"SP_PROCCHG_INSERT_LINK", "IKE"},  // Count: 1, Ordinal: 3531
            {"SP_PROCCHG_INSERT_PROCCHGHISTORY", "JKE"},  // Count: 1, Ordinal: 3532
            {"SP_PROCCHG_SELECT_BASE_UNIT_ALL", "KKE"},  // Count: 1, Ordinal: 3533
            {"SP_PROCCHG_SELECT_BASE_UNIT_BY_GROUP", "LKE"},  // Count: 1, Ordinal: 3534
            {"SP_PROCCHG_SELECT_BASE_UNIT_BY_UNIT", "MKE"},  // Count: 1, Ordinal: 3535
            {"SP_PROCCHG_SELECT_LINKED_UNITS_BY_BASE_UNIT", "ALE"},  // Count: 1, Ordinal: 3536
            {"SP_PROCCHG_SELECT_LINKS_BY_BASEUNIT", "BLE"},  // Count: 1, Ordinal: 3537
            {"SP_PROCCHG_SELECT_LINKS_BY_GROUP", "CLE"},  // Count: 1, Ordinal: 3538
            {"SP_PROCCHG_SELECT_LINKS_BY_GROUP_BASEUNIT_LINKEDUNIT", "DLE"},  // Count: 1, Ordinal: 3539
            {"SP_PURGE_YINVADJ_TABLES", "ELE"},  // Count: 1, Ordinal: 3540
            {"SP_SELECT_ACCESS", "FLE"},  // Count: 1, Ordinal: 3541
            {"SP_SELECT_ADMIN_BY_SERVER", "GLE"},  // Count: 1, Ordinal: 3542
            {"SP_SELECT_APPLICATION_1", "HLE"},  // Count: 1, Ordinal: 3543
            {"SP_SELECT_CAUSEACTION", "ILE"},  // Count: 1, Ordinal: 3544
            {"SP_SELECT_CAUSECHILDREN", "JLE"},  // Count: 1, Ordinal: 3545
            {"SP_SELECT_CS_TO_PI_1", "KLE"},  // Count: 1, Ordinal: 3546
            {"SP_SELECT_EQUIPMENT_1", "LLE"},  // Count: 1, Ordinal: 3547
            {"SP_SELECT_GROUP_1", "MLE"},  // Count: 1, Ordinal: 3548
            {"SP_SELECT_GROUP_BY_GROUP", "AME"},  // Count: 1, Ordinal: 3549
            {"SP_SELECT_LIST_1", "BME"},  // Count: 1, Ordinal: 3550
            {"SP_SELECT_LIST_AREA_1", "CME"},  // Count: 1, Ordinal: 3551
            {"SP_SELECT_LIST_AREA_BY_GROUP", "DME"},  // Count: 1, Ordinal: 3552
            {"SP_SELECT_LISTLISTPOINTSANDTAGS", "EME"},  // Count: 1, Ordinal: 3553
            {"SP_SELECT_NEXTSORT", "FME"},  // Count: 1, Ordinal: 3554
            {"SP_SELECT_NOOFREMINDERMAILS_BY_QUARTER_YEAR", "GME"},  // Count: 1, Ordinal: 3555
            {"SP_SELECT_ORDERED_LIST_LIST_1", "HME"},  // Count: 1, Ordinal: 3556
            {"SP_SELECT_PI_1", "IME"},  // Count: 1, Ordinal: 3557
            {"SP_SELECT_PIMSCONTACTACTION_BY_QUARTER_YEAR", "JME"},  // Count: 1, Ordinal: 3558
            {"SP_SELECT_POINTS_AND_TAGS_1", "KME"},  // Count: 1, Ordinal: 3559
            {"SP_SELECT_POINTS_AND_TAGS_2", "LME"},  // Count: 1, Ordinal: 3560
            {"SP_SELECT_POINTS_AND_TAGS_BY_CGTYPE", "MME"},  // Count: 1, Ordinal: 3561
            {"SP_SELECT_PRINTING_1", "ANE"},  // Count: 1, Ordinal: 3562
            {"SP_SELECT_PROCESS_LIMITS_ALL_UNIQ", "BNE"},  // Count: 1, Ordinal: 3563
            {"SP_SELECT_PROCESS_LIMITS_ALL_UNIQ_NEW", "CNE"},  // Count: 1, Ordinal: 3564
            {"SP_SELECT_PROCESS_LIMITS_BY_TAG", "DNE"},  // Count: 1, Ordinal: 3565
            {"SP_SELECT_PROCESS_LIMITS_BY_UNIT", "ENE"},  // Count: 1, Ordinal: 3566
            {"SP_SELECT_PROCESS_LIMITS_BY_UNIT_UNIQ", "FNE"},  // Count: 1, Ordinal: 3567
            {"SP_SELECT_PROCESS_LIMITS_DETAIL", "GNE"},  // Count: 1, Ordinal: 3568
            {"SP_SELECT_PROCESS_LIMITS_DETAIL_NEW", "HNE"},  // Count: 1, Ordinal: 3569
            {"SP_SELECT_PROCESS_LIMITS_PROD_UNIQ", "INE"},  // Count: 1, Ordinal: 3570
            {"SP_SELECT_PROCESS_LIMITS_STATE_UNIQ", "JNE"},  // Count: 1, Ordinal: 3571
            {"SP_SELECT_PRODUCTION_UNIT_ALL", "KNE"},  // Count: 1, Ordinal: 3572
            {"SP_SELECT_PRODUCTION_UNIT_BY_GROUP", "LNE"},  // Count: 1, Ordinal: 3573
            {"SP_SELECT_PRODUCTION_UNIT_BY_UNIT", "MNE"},  // Count: 1, Ordinal: 3574
            {"SP_SELECT_PRODUCTION_UNIT_BY_UNIT_NEW", "AOE"},  // Count: 1, Ordinal: 3575
            {"SP_SELECT_RDMGROUP_BY_GROUP", "BOE"},  // Count: 1, Ordinal: 3576
            {"SP_SELECT_RDMGROUP_BY_SERVER", "COE"},  // Count: 1, Ordinal: 3577
            {"SP_SELECT_RDMGROUP_BY_SERVER_GROUP", "DOE"},  // Count: 1, Ordinal: 3578
            {"SP_SELECT_RDMSERVER_ALL_SERVERS", "EOE"},  // Count: 1, Ordinal: 3579
            {"SP_SELECT_RDMSERVER_BY_SERVER", "FOE"},  // Count: 1, Ordinal: 3580
            {"SP_SELECT_RDMTAG_BY_SERVER_GROUP_TAG", "GOE"},  // Count: 1, Ordinal: 3581
            {"SP_SELECT_RDMTAGS_BY_GROUP", "HOE"},  // Count: 1, Ordinal: 3582
            {"SP_SELECT_RDMTAGS_BY_TAGS", "IOE"},  // Count: 1, Ordinal: 3583
            {"SP_SELECT_RDMTEST", "JOE"},  // Count: 1, Ordinal: 3584
            {"SP_SELECT_RULESBYTYPE", "KOE"},  // Count: 1, Ordinal: 3585
            {"SP_SELECT_SORT", "LOE"},  // Count: 1, Ordinal: 3586
            {"SP_SELECT_TYPES", "MOE"},  // Count: 1, Ordinal: 3587
            {"SP_TRACE_CREATE", "APE"},  // Count: 1, Ordinal: 3588
            {"SP_UPDATE_APPLICATION_1", "BPE"},  // Count: 1, Ordinal: 3589
            {"SP_UPDATE_CS_TO_PI_1", "CPE"},  // Count: 1, Ordinal: 3590
            {"SP_UPDATE_CS_TO_PIGROUP_1", "DPE"},  // Count: 1, Ordinal: 3591
            {"SP_UPDATE_EQUIPMENT_1", "EPE"},  // Count: 1, Ordinal: 3592
            {"SP_UPDATE_GROUP_1", "FPE"},  // Count: 1, Ordinal: 3593
            {"SP_UPDATE_GROUP_DES", "GPE"},  // Count: 1, Ordinal: 3594
            {"SP_UPDATE_INSERT_PROCESS_LIMITS", "HPE"},  // Count: 1, Ordinal: 3595
            {"SP_UPDATE_LIMITS_GROUP_1", "IPE"},  // Count: 1, Ordinal: 3596
            {"SP_UPDATE_LIST_1", "JPE"},  // Count: 1, Ordinal: 3597
            {"SP_UPDATE_LIST_AREA_1", "KPE"},  // Count: 1, Ordinal: 3598
            {"SP_UPDATE_LIST_POINTS", "LPE"},  // Count: 1, Ordinal: 3599
            {"SP_UPDATE_LIST_POINTS_1", "MPE"},  // Count: 1, Ordinal: 3600
            {"SP_UPDATE_LISTPOINTSANDTAGS", "AQE"},  // Count: 1, Ordinal: 3601
            {"SP_UPDATE_NOOFREMINDERMAILS_BY_QUARTER_YEAR", "BQE"},  // Count: 1, Ordinal: 3602
            {"SP_UPDATE_ORDERED_LIST_1", "CQE"},  // Count: 1, Ordinal: 3603
            {"SP_UPDATE_ORDERED_LIST_LIST_1", "DQE"},  // Count: 1, Ordinal: 3604
            {"SP_UPDATE_PI_1", "EQE"},  // Count: 1, Ordinal: 3605
            {"SP_UPDATE_PROCESS_LIMITS_1", "FQE"},  // Count: 1, Ordinal: 3606
            {"SP_UPDATE_PROCESS_LIMITS_TAG", "GQE"},  // Count: 1, Ordinal: 3607
            {"SP_UPDATE_PRODUCTION_UNIT", "HQE"},  // Count: 1, Ordinal: 3608
            {"SP_UPDATE_PRODUCTION_UNIT_1", "IQE"},  // Count: 1, Ordinal: 3609
            {"SP_UPDATE_PRODUCTION_UNIT_WCOMMENT", "JQE"},  // Count: 1, Ordinal: 3610
            {"SP_UPDATE_PRODUCTION_UNIT_WCOMMENT2", "KQE"},  // Count: 1, Ordinal: 3611
            {"SP_UPDATE_PRODUCTION_UNIT_WCOMMENT2_NEW", "LQE"},  // Count: 1, Ordinal: 3612
            {"SP_UPDATE_PRODUCTION_UNIT_WCOMMENTXX", "MQE"},  // Count: 1, Ordinal: 3613
            {"SP_UPDATE_RDMGROUP", "ARE"},  // Count: 1, Ordinal: 3614
            {"SP_UPDATE_RDMGROUP_ALARMSTATUS", "BRE"},  // Count: 1, Ordinal: 3615
            {"SP_UPDATE_RDMSERVER", "CRE"},  // Count: 1, Ordinal: 3616
            {"SP_UPDATE_RDMTAGS", "DRE"},  // Count: 1, Ordinal: 3617
            {"SP_UPDATE_RDMTAGS_2", "ERE"},  // Count: 1, Ordinal: 3618
            {"SP_UPDATE_RDMTAGS_ALARM_SCANTIME", "FRE"},  // Count: 1, Ordinal: 3619
            {"SP_UPDATE_RDMTAGS_SCANTIME", "GRE"},  // Count: 1, Ordinal: 3620
            {"SP_UPDATE_RDMTAGS_VALIDTAGVALUE", "HRE"},  // Count: 1, Ordinal: 3621
            {"SP_UPDATE_RENAME_PRODUCTION_UNIT", "IRE"},  // Count: 1, Ordinal: 3622
            {"SP_UPDATE_RULETYPES", "JRE"},  // Count: 1, Ordinal: 3623
            {"SP_UPDATE_TAGS_1", "KRE"},  // Count: 1, Ordinal: 3624
            {"SP_UPDATE_TYPES", "LRE"},  // Count: 1, Ordinal: 3625
            {"SP_UPDATEKEYGROUP", "MRE"},  // Count: 1, Ordinal: 3626
            {"SP_UPDATEYINVADJ_INVENTORYPITAGS", "ASE"},  // Count: 1, Ordinal: 3627
            {"SP_UPDATEYINVADJ_PROCMSGDATA", "BSE"},  // Count: 1, Ordinal: 3628
            {"SP_WORK", "CSE"},  // Count: 1, Ordinal: 3629
            {"SPC_LIMITS", "DSE"},  // Count: 1, Ordinal: 3630
            {"SQ'", "ESE"},  // Count: 1, Ordinal: 3631
            {"'SQL TRACE FILE PATH IS NOT CORRECT OR DOES NOT EXIST'", "FSE"},  // Count: 1, Ordinal: 3632
            {"'SQL TRACE INFO DOES NOT EXIST'", "GSE"},  // Count: 1, Ordinal: 3633
            {"STOREDPROCEDURE1", "HSE"},  // Count: 1, Ordinal: 3634
            {"SWAP_TRACE", "ISE"},  // Count: 1, Ordinal: 3635
            {"SYSTEM", "JSE"},  // Count: 1, Ordinal: 3636
            {"SYSTEM_AVAIL", "KSE"},  // Count: 1, Ordinal: 3637
            {"TABLE", "LSE"},  // Count: 1, Ordinal: 3638
            {"'TEXT'", "MSE"},  // Count: 1, Ordinal: 3639
            {"TIME", "ATE"},  // Count: 1, Ordinal: 3640
            {"TIME_STAMP", "BTE"},  // Count: 1, Ordinal: 3641
            {"TIMEWRITTENTORLINKDB", "CTE"},  // Count: 1, Ordinal: 3642
            {"TO_STORAGE_LOCATION", "DTE"},  // Count: 1, Ordinal: 3643
            {"TOTALINVENTORYQUANTITY", "ETE"},  // Count: 1, Ordinal: 3644
            {"TRACE_DETAIL", "FTE"},  // Count: 1, Ordinal: 3645
            {"TRANSACTION_DATETIME", "GTE"},  // Count: 1, Ordinal: 3646
            {"TRANSDATE", "HTE"},  // Count: 1, Ordinal: 3647
            {"TRC'", "ITE"},  // Count: 1, Ordinal: 3648
            {"'TRUE'", "JTE"},  // Count: 1, Ordinal: 3649
            {"TS'", "KTE"},  // Count: 1, Ordinal: 3650
            {"UP_TIME", "LTE"},  // Count: 1, Ordinal: 3651
            {"'UPDATE 1'", "MTE"},  // Count: 1, Ordinal: 3652
            {"USAGE_PARTS", "AUE"},  // Count: 1, Ordinal: 3653
            {"'USE %'", "BUE"},  // Count: 1, Ordinal: 3654
            {"USERACTION", "CUE"},  // Count: 1, Ordinal: 3655
            {"USERNAME", "DUE"},  // Count: 1, Ordinal: 3656
            {"USP_CS_COPY_ACTION_RESETTAGS", "EUE"},  // Count: 1, Ordinal: 3657
            {"USP_CS_COPY_ACTIONS_RESETTAGS_RETURN_CAUSERELATIONS", "FUE"},  // Count: 1, Ordinal: 3658
            {"USP_CS_COPY_CAUSE_CREATE_CAUSERELATION", "GUE"},  // Count: 1, Ordinal: 3659
            {"USP_CS_COPY_CAUSE_CREATE_CAUSERELATION1", "HUE"},  // Count: 1, Ordinal: 3660
            {"USP_CS_COPY_CAUSE_LINK_TO_STRATEGY", "IUE"},  // Count: 1, Ordinal: 3661
            {"USP_CS_COPY_CAUSE_LINK_TO_STRATEGY1", "JUE"},  // Count: 1, Ordinal: 3662
            {"USP_CS_COPY_STRATEGY_RETURN_ROOTCAUSES", "KUE"},  // Count: 1, Ordinal: 3663
            {"USP_CS_DELETE_ACTION", "LUE"},  // Count: 1, Ordinal: 3664
            {"USP_CS_DELETE_CAUSE_ACTIONS_RESETTAGS", "MUE"},  // Count: 1, Ordinal: 3665
            {"USP_CS_DELETE_CAUSE_ACTIONS_RESETTAGS1", "AVE"},  // Count: 1, Ordinal: 3666
            {"USP_CS_DELETE_CSSCHEDULINGINFO", "BVE"},  // Count: 1, Ordinal: 3667
            {"USP_CS_DELETE_CSSTRATEGIES_BY_STRATEGYID", "CVE"},  // Count: 1, Ordinal: 3668
            {"USP_CS_DELETE_CSSTRATEGYTAGS_BY_STRATEGYID_TAGNAME", "DVE"},  // Count: 1, Ordinal: 3669
            {"USP_CS_DELETE_CSSUPPLEMENTALDATA", "EVE"},  // Count: 1, Ordinal: 3670
            {"USP_CS_DELETE_LINKED_TREE_CHECK", "FVE"},  // Count: 1, Ordinal: 3671
            {"USP_CS_DELETE_RULES_BY_RULEID", "GVE"},  // Count: 1, Ordinal: 3672
            {"USP_CS_GETALLCITATIONHISTORYBYSTRAGEGYID", "HVE"},  // Count: 1, Ordinal: 3673
            {"USP_CS_INSERT_CSCRITERIA", "IVE"},  // Count: 1, Ordinal: 3674
            {"USP_CS_INSERT_CSCRITERIA_4", "JVE"},  // Count: 1, Ordinal: 3675
            {"USP_CS_INSERT_CSCRITERIA_5", "KVE"},  // Count: 1, Ordinal: 3676
            {"USP_CS_INSERT_CSRULES", "LVE"},  // Count: 1, Ordinal: 3677
            {"USP_CS_INSERT_CSRULES_2", "MVE"},  // Count: 1, Ordinal: 3678
            {"USP_CS_INSERT_CSRULES_3", "AWE"},  // Count: 1, Ordinal: 3679
            {"USP_CS_INSERT_CSRULES_4", "BWE"},  // Count: 1, Ordinal: 3680
            {"USP_CS_INSERT_CSRULES_6", "CWE"},  // Count: 1, Ordinal: 3681
            {"USP_CS_INSERT_CSRULES_6_NEW", "DWE"},  // Count: 1, Ordinal: 3682
            {"USP_CS_INSERT_CSRULES_7", "EWE"},  // Count: 1, Ordinal: 3683
            {"USP_CS_INSERT_CSRULES_8", "FWE"},  // Count: 1, Ordinal: 3684
            {"USP_CS_INSERT_CSSUPPLEMENTALDATA", "GWE"},  // Count: 1, Ordinal: 3685
            {"USP_CS_INSERT_CSSUPPLEMENTALDATA_1", "HWE"},  // Count: 1, Ordinal: 3686
            {"USP_CS_INSERT_EXIST_CHILD_CHECK", "IWE"},  // Count: 1, Ordinal: 3687
            {"USP_CS_INSERT_EXIST_ROOT_CHECK", "JWE"},  // Count: 1, Ordinal: 3688
            {"USP_CS_INSERT_UPDATE_CSSCHEDULINGINFO", "KWE"},  // Count: 1, Ordinal: 3689
            {"USP_CS_REMOVE_LINKED_CAUSE", "LWE"},  // Count: 1, Ordinal: 3690
            {"USP_CS_SELECT_CRITERIA", "MWE"},  // Count: 1, Ordinal: 3691
            {"USP_CS_SELECT_CRITERIA_BY_RULEID", "AXE"},  // Count: 1, Ordinal: 3692
            {"USP_CS_SELECT_CRITERIA_BY_RULEID_5", "BXE"},  // Count: 1, Ordinal: 3693
            {"USP_CS_SELECT_CSACTIONMASTER_BY_ACTIONID", "CXE"},  // Count: 1, Ordinal: 3694
            {"USP_CS_SELECT_CSACTIONTAGS_BY_ACTIONID", "DXE"},  // Count: 1, Ordinal: 3695
            {"USP_CS_SELECT_CSCAUSEACTION_BY_CAUSEID", "EXE"},  // Count: 1, Ordinal: 3696
            {"USP_CS_SELECT_CSCAUSERELATIONS_BY_PARENTCAUSEID", "FXE"},  // Count: 1, Ordinal: 3697
            {"USP_CS_SELECT_CSCAUSES_BY_CAUSEID", "GXE"},  // Count: 1, Ordinal: 3698
            {"USP_CS_SELECT_CSRECENTACTIONDAYS", "HXE"},  // Count: 1, Ordinal: 3699
            {"USP_CS_SELECT_CSRULEHISTORY", "IXE"},  // Count: 1, Ordinal: 3700
            {"USP_CS_SELECT_CSRULEOPERATORS_BY_OPERATORID", "JXE"},  // Count: 1, Ordinal: 3701
            {"USP_CS_SELECT_CSSCHEDULINGINFO", "KXE"},  // Count: 1, Ordinal: 3702
            {"USP_CS_SELECT_CSSTRATEGIES_BY_STRATEGYID", "LXE"},  // Count: 1, Ordinal: 3703
            {"USP_CS_SELECT_CSSTRATEGYCAUSES_BY_STRATEGYID", "MXE"},  // Count: 1, Ordinal: 3704
            {"USP_CS_SELECT_CSSTRATEGYTAGS_BY_STRATEGYID", "AYE"},  // Count: 1, Ordinal: 3705
            {"USP_CS_SELECT_CSSTRATEGYTAGS_BY_STRATEGYID_TAGNAME", "BYE"},  // Count: 1, Ordinal: 3706
            {"USP_CS_SELECT_DISTINCT_TAGS_FROM_CSCRITERIA", "CYE"},  // Count: 1, Ordinal: 3707
            {"USP_CS_SELECT_OPENCITATIONS_BY_RULEID", "DYE"},  // Count: 1, Ordinal: 3708
            {"USP_CS_SELECT_RULES", "EYE"},  // Count: 1, Ordinal: 3709
            {"USP_CS_SELECT_RULES_2", "FYE"},  // Count: 1, Ordinal: 3710
            {"USP_CS_SELECT_RULES_2_NEW", "GYE"},  // Count: 1, Ordinal: 3711
            {"USP_CS_SELECT_RULES_2_NEW_1", "HYE"},  // Count: 1, Ordinal: 3712
            {"USP_CS_SELECT_RULES_2_NEW_2", "IYE"},  // Count: 1, Ordinal: 3713
            {"USP_CS_SELECT_RULES_3", "JYE"},  // Count: 1, Ordinal: 3714
            {"USP_CS_SELECT_RULES_3_NEW", "KYE"},  // Count: 1, Ordinal: 3715
            {"USP_CS_SELECT_RULES_3_NEW_1", "LYE"},  // Count: 1, Ordinal: 3716
            {"USP_CS_SELECT_RULES_3_NEW_2", "MYE"},  // Count: 1, Ordinal: 3717
            {"USP_CS_SELECT_RULES_SORT_DESC", "AZE"},  // Count: 1, Ordinal: 3718
            {"USP_CS_SELECT_RULES_SORTDESCRIPTION", "BZE"},  // Count: 1, Ordinal: 3719
            {"USP_CS_SELECT_RULES_SORTSTRATEGY", "CZE"},  // Count: 1, Ordinal: 3720
            {"USP_CS_SELECTALL_AREAS", "DZE"},  // Count: 1, Ordinal: 3721
            {"USP_CS_SELECTALL_CRITERIATYPES", "EZE"},  // Count: 1, Ordinal: 3722
            {"USP_CS_SELECTALL_MAILBOXNAMES", "FZE"},  // Count: 1, Ordinal: 3723
            {"USP_CS_SELECTALL_RULELOGICALS", "GZE"},  // Count: 1, Ordinal: 3724
            {"USP_CS_SELECTALL_RULEOPERATORS", "HZE"},  // Count: 1, Ordinal: 3725
            {"USP_CS_SELECTALL_STRATEGIES", "IZE"},  // Count: 1, Ordinal: 3726
            {"USP_CS_SELECTALL_SUPPLEMENTALACTIONS", "JZE"},  // Count: 1, Ordinal: 3727
            {"USP_CS_UPDATE_CSCRITERIA_BY_ALIAS", "KZE"},  // Count: 1, Ordinal: 3728
            {"USP_CS_UPDATE_CSRESETSCHEDULINGALARM", "LZE"},  // Count: 1, Ordinal: 3729
            {"USP_CS_UPDATE_CSRULES", "MZE"},  // Count: 1, Ordinal: 3730
            {"USP_CS_UPDATE_CSRULES_2", "AaE"},  // Count: 1, Ordinal: 3731
            {"USP_CS_UPDATE_CSRULES_3", "BaE"},  // Count: 1, Ordinal: 3732
            {"USP_CS_UPDATE_CSRULES_4", "CaE"},  // Count: 1, Ordinal: 3733
            {"USP_CS_UPDATE_CSRULES_6", "DaE"},  // Count: 1, Ordinal: 3734
            {"USP_CS_UPDATE_CSRULES_6_NEW", "EaE"},  // Count: 1, Ordinal: 3735
            {"USP_CS_UPDATE_CSRULES_7", "FaE"},  // Count: 1, Ordinal: 3736
            {"USP_CS_UPDATE_CSRULES_8", "GaE"},  // Count: 1, Ordinal: 3737
            {"USP_CS_UPDATE_CSSTRATEGYTAGS_BY_STRATEGYID_TAGNAME", "HaE"},  // Count: 1, Ordinal: 3738
            {"USP_CSMAILBOXES_PURGE", "IaE"},  // Count: 1, Ordinal: 3739
            {"USP_CSMAILBOXES_PURGEALLMAIL", "JaE"},  // Count: 1, Ordinal: 3740
            {"USP_CSRULES_SEL_RULEID_TAGNAMEBYMAILBOXID", "KaE"},  // Count: 1, Ordinal: 3741
            {"USP_GET_SAP_LOGON", "LaE"},  // Count: 1, Ordinal: 3742
            {"USP_GETDATAALARMHISTORYREPORT", "MaE"},  // Count: 1, Ordinal: 3743
            {"USP_GSPRODUCTLIMITS_INSERT", "AbE"},  // Count: 1, Ordinal: 3744
            {"USP_GSPRODUCTLIMITS_SELECT_BY_PRODUCT", "BbE"},  // Count: 1, Ordinal: 3745
            {"USP_GSPRODUCTLIMITS_SELECT_BY_PRODUCT_UNIT_STAGE", "CbE"},  // Count: 1, Ordinal: 3746
            {"USP_GSPRODUCTLIMITS_UPDATE", "DbE"},  // Count: 1, Ordinal: 3747
            {"USP_PCBO_COMPARE_GMNS", "EbE"},  // Count: 1, Ordinal: 3748
            {"USP_PCBO_DELETE_BATCH", "FbE"},  // Count: 1, Ordinal: 3749
            {"USP_PCBO_DELETE_RESOURCEBATCH", "GbE"},  // Count: 1, Ordinal: 3750
            {"USP_PCBO_DELETE_SAMPLE", "HbE"},  // Count: 1, Ordinal: 3751
            {"USP_PCBO_GET_AREADETAILS", "IbE"},  // Count: 1, Ordinal: 3752
            {"USP_PCBO_GET_AREAS", "JbE"},  // Count: 1, Ordinal: 3753
            {"USP_PCBO_GET_BATCHPART", "KbE"},  // Count: 1, Ordinal: 3754
            {"USP_PCBO_GET_BLENDING", "LbE"},  // Count: 1, Ordinal: 3755
            {"USP_PCBO_GET_DCSBATCHMODULE", "MbE"},  // Count: 1, Ordinal: 3756
            {"USP_PCBO_GET_DETAILED_PRODUCTION", "AcE"},  // Count: 1, Ordinal: 3757
            {"USP_PCBO_GET_EMAIL_FOR_ERRORS", "BcE"},  // Count: 1, Ordinal: 3758
            {"USP_PCBO_GET_EST_PRODUCTION", "CcE"},  // Count: 1, Ordinal: 3759
            {"USP_PCBO_GET_GMN_DETAILS", "DcE"},  // Count: 1, Ordinal: 3760
            {"USP_PCBO_GET_GMN_DETAILSBYPMGRADE", "EcE"},  // Count: 1, Ordinal: 3761
            {"USP_PCBO_GET_GMN_INSPECTION_TYPE", "FcE"},  // Count: 1, Ordinal: 3762
            {"USP_PCBO_GET_GMNS", "GcE"},  // Count: 1, Ordinal: 3763
            {"USP_PCBO_GET_LABOR_EQUIP", "HcE"},  // Count: 1, Ordinal: 3764
            {"USP_PCBO_GET_NOBATCHVAL_GMNS", "IcE"},  // Count: 1, Ordinal: 3765
            {"USP_PCBO_GET_PACKAGE_BYBATCH_VERIFICATION", "JcE"},  // Count: 1, Ordinal: 3766
            {"USP_PCBO_GET_PACKAGE_GMNS", "KcE"},  // Count: 1, Ordinal: 3767
            {"USP_PCBO_GET_PACKAGE_VERIFICATION", "LcE"},  // Count: 1, Ordinal: 3768
            {"USP_PCBO_GET_PARENT_GMN", "McE"},  // Count: 1, Ordinal: 3769
            {"USP_PCBO_GET_PD_TAGS", "AdE"},  // Count: 1, Ordinal: 3770
            {"USP_PCBO_GET_PMGR_DETAILS", "BdE"},  // Count: 1, Ordinal: 3771
            {"USP_PCBO_GET_PREVIOUS_BATCHES", "CdE"},  // Count: 1, Ordinal: 3772
            {"USP_PCBO_GET_PREVIOUS_BATCHES_ORDERBYSTARTTIME", "DdE"},  // Count: 1, Ordinal: 3773
            {"USP_PCBO_GET_PREVIOUS_RESOURCE_BATCHES", "EdE"},  // Count: 1, Ordinal: 3774
            {"USP_PCBO_GET_PROC_MESSAGES", "FdE"},  // Count: 1, Ordinal: 3775
            {"USP_PCBO_GET_PRODUCTION", "GdE"},  // Count: 1, Ordinal: 3776
            {"USP_PCBO_GET_PRODUCTION_BY_PART", "HdE"},  // Count: 1, Ordinal: 3777
            {"USP_PCBO_GET_RESOURCE_DETAILS", "IdE"},  // Count: 1, Ordinal: 3778
            {"USP_PCBO_GET_SAMPLES", "JdE"},  // Count: 1, Ordinal: 3779
            {"USP_PCBO_GET_SAMPLES_BYBATCH_VERIFICATION", "KdE"},  // Count: 1, Ordinal: 3780
            {"USP_PCBO_GET_SAMPLES_WPART", "LdE"},  // Count: 1, Ordinal: 3781
            {"USP_PCBO_GET_SAMPLES_WTEMPLATE", "MdE"},  // Count: 1, Ordinal: 3782
            {"USP_PCBO_GET_SAPAVAILABILITY", "AeE"},  // Count: 1, Ordinal: 3783
            {"USP_PCBO_GET_TOSTORAGELOC", "BeE"},  // Count: 1, Ordinal: 3784
            {"USP_PCBO_GET_USAGE", "CeE"},  // Count: 1, Ordinal: 3785
            {"USP_PCBO_GET_USAGE_BYBATCH_VERIFICATION", "DeE"},  // Count: 1, Ordinal: 3786
            {"USP_PCBO_GET_USAGE_VERIFICATION", "EeE"},  // Count: 1, Ordinal: 3787
            {"USP_PCBO_INSERT_BATCH", "FeE"},  // Count: 1, Ordinal: 3788
            {"USP_PCBO_INSERT_BLENDING", "GeE"},  // Count: 1, Ordinal: 3789
            {"USP_PCBO_INSERT_DETAILED_PRODUCTION_TRACKING", "HeE"},  // Count: 1, Ordinal: 3790
            {"USP_PCBO_INSERT_EST_PRODUCTION", "IeE"},  // Count: 1, Ordinal: 3791
            {"USP_PCBO_INSERT_LABOR_EQUIP", "JeE"},  // Count: 1, Ordinal: 3792
            {"USP_PCBO_INSERT_PRODUCTION", "KeE"},  // Count: 1, Ordinal: 3793
            {"USP_PCBO_INSERT_RAWUSAGE", "LeE"},  // Count: 1, Ordinal: 3794
            {"USP_PCBO_INSERT_TRANSACTION_LOG", "MeE"},  // Count: 1, Ordinal: 3795
            {"USP_PCBO_INSERT_UPDATE_SAMPLE_DETAILS", "AfE"},  // Count: 1, Ordinal: 3796
            {"USP_PCBO_INSERT_USAGE", "BfE"},  // Count: 1, Ordinal: 3797
            {"USP_PCBO_UPDATE_PART", "CfE"},  // Count: 1, Ordinal: 3798
            {"USP_SAF_CHART_INS", "DfE"},  // Count: 1, Ordinal: 3799
            {"USP_SAF_CHART_SEL_BY_KEY", "EfE"},  // Count: 1, Ordinal: 3800
            {"USP_SAF_CHART_UPD_BY_KEY", "FfE"},  // Count: 1, Ordinal: 3801
            {"USP_SAF_GROUP_DEL_BY_KEY", "GfE"},  // Count: 1, Ordinal: 3802
            {"USP_SAF_GROUP_INS", "HfE"},  // Count: 1, Ordinal: 3803
            {"USP_SAF_GROUP_QUERY_XREF_DEL_BY_GROUP_ID", "IfE"},  // Count: 1, Ordinal: 3804
            {"USP_SAF_GROUP_QUERY_XREF_DEL_BY_KEY", "JfE"},  // Count: 1, Ordinal: 3805
            {"USP_SAF_GROUP_QUERY_XREF_INS", "KfE"},  // Count: 1, Ordinal: 3806
            {"USP_SAF_GROUP_SEL_BY_USERID", "LfE"},  // Count: 1, Ordinal: 3807
            {"USP_SAF_LIMS_CRITERIA_INS", "MfE"},  // Count: 1, Ordinal: 3808
            {"USP_SAF_LIMS_CRITERIA_INS_EX", "AgE"},  // Count: 1, Ordinal: 3809
            {"USP_SAF_LIMS_CRITERIA_SEL_BY_KEY", "BgE"},  // Count: 1, Ordinal: 3810
            {"USP_SAF_PIMS_CRITERIA_INS", "CgE"},  // Count: 1, Ordinal: 3811
            {"USP_SAF_PIMS_CRITERIA_INS_1", "DgE"},  // Count: 1, Ordinal: 3812
            {"USP_SAF_PIMS_CRITERIA_SEL_BY_KEY", "EgE"},  // Count: 1, Ordinal: 3813
            {"USP_SAF_QUERY_INS", "FgE"},  // Count: 1, Ordinal: 3814
            {"USP_SAF_QUERY_SEL_BY_QUERY_ID", "GgE"},  // Count: 1, Ordinal: 3815
            {"USP_SELECT_SAF_QUERIES_BY_GROUP", "HgE"},  // Count: 1, Ordinal: 3816
            {"WEEK", "IgE"},  // Count: 1, Ordinal: 3817
            {"WINDOWS_UID", "JgE"},  // Count: 1, Ordinal: 3818
            {"XACT_ABORT", "KgE"},  // Count: 1, Ordinal: 3819
            {"'YINVADJ'", "LgE"},  // Count: 1, Ordinal: 3820
            {"'YINVADJ_RLINK'", "MgE"},  // Count: 1, Ordinal: 3821
            {"YQA01_INSPECTION_TYPE", "AhE"}  // Count: 1, Ordinal: 3822

        };
    }
}
