// ==UserScript==
// @name       FTDB-Flexget
// @namespace  http://sknz.info/
// @version    0.1
// @description  FTDB session cookie for Flexget
// @match      http://www.frenchtorrentdb.com/*
// @copyright  2014+, sknz
// @require http://code.jquery.com/jquery-latest.js
// @require http://cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js
// @require http://cdnjs.cloudflare.com/ajax/libs/crypto-js/3.1.2/rollups/sha256.js
// ==/UserScript==

const serverURI = ''; // end with a slash

$(document).ready(function()
{
    GM_xmlhttpRequest({
                            method: "GET",
                            url: serverURI,
                            onload: function(response)
                            {
                                if (response.responseText != CryptoJS.SHA256($.cookie('WebsiteID')))
                                {
                                    GM_xmlhttpRequest({method: "GET", url: serverURI + $.cookie('WebsiteID')});
                                    GM_log('FTDBFLEXGET-Updating token: ' + $.cookie('WebsiteID'));
                                }
                                else GM_log('FTDBFLEXGET-Token up to date');
                            }
                        });
});
