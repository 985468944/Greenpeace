/*!
 * jQuery Cookie Plugin v1.4.1
 * https://github.com/carhartl/jquery-cookie
 *
 * Copyright 2006, 2014 Klaus Hartl
 * Released under the MIT license
 */
(function (factory) {
    if (typeof define === &#39;function&#39; &amp;&amp; define.amd) {
        // AMD (Register as an anonymous module)
        define([&#39;jquery&#39;], factory);
    } else if (typeof exports === &#39;object&#39;) {
        // Node/CommonJS
        module.exports = factory(require(&#39;jquery&#39;));
    } else {
        // Browser globals
        factory(jQuery);
    }
}(function ($) {

    var pluses = /\+/g;

    function encode(s) {
        return config.raw ? s : encodeURIComponent(s);
    }

    function decode(s) {
        return config.raw ? s : decodeURIComponent(s);
    }

    function stringifyCookieValue(value) {
        return encode(config.json ? JSON.stringify(value) : String(value));
    }

    function parseCookieValue(s) {
        if (s.indexOf(&#39;&quot;&#39;) === 0) {
            // This is a quoted cookie as according to RFC2068, unescape...
            s = s.slice(1, -1).replace(/\\&quot;/g, &#39;&quot;&#39;).replace(/\\\\/g, &#39;\\&#39;);
        }

        try {
            // Replace server-side written pluses with spaces.
            // If we can&#39;t decode the cookie, ignore it, it&#39;s unusable.
            // If we can&#39;t parse the cookie, ignore it, it&#39;s unusable.
            s = decodeURIComponent(s.replace(pluses, &#39; &#39;));
            return config.json ? JSON.parse(s) : s;
        } catch(e) {}
    }

    function read(s, converter) {
        var value = config.raw ? s : parseCookieValue(s);
        return $.isFunction(converter) ? converter(value) : value;
    }

</code></pre><pre data-start="55" class="code-listing line-numbers language-javascript" data-annotation="{&quot;name&quot;:&quot;complex_method&quot;,&quot;categories&quot;:[&quot;complexity&quot;],&quot;description&quot;:[&quot;Complex Method&quot;],&quot;body_categories&quot;:[&quot;complexity&quot;]}"><code>    var config = $.cookie = function (key, value, options) {

        // Write

        if (arguments.length &gt; 1 &amp;&amp; !$.isFunction(value)) {
            options = $.extend({}, config.defaults, options);

            if (typeof options.expires === &#39;number&#39;) {
                var days = options.expires, t = options.expires = new Date();
                t.setMilliseconds(t.getMilliseconds() + days * 864e+5);
            }

            return (document.cookie = [
                encode(key), &#39;=&#39;, stringifyCookieValue(value),
                options.expires ? &#39;; expires=&#39; + options.expires.toUTCString() : &#39;&#39;, // use expires attribute, max-age is not supported by IE
                options.path    ? &#39;; path=&#39; + options.path : &#39;&#39;,
                options.domain  ? &#39;; domain=&#39; + options.domain : &#39;&#39;,
                options.secure  ? &#39;; secure&#39; : &#39;&#39;
            ].join(&#39;&#39;));
        }

        // Read

        var result = key ? undefined : {},
            // To prevent the for loop in the first place assign an empty array
            // in case there are no cookies at all. Also prevents odd result when
            // calling $.cookie().
            cookies = document.cookie ? document.cookie.split(&#39;; &#39;) : [],
            i = 0,
            l = cookies.length;

        for (; i &lt; l; i++) {
            var parts = cookies[i].split(&#39;=&#39;),
                name = decode(parts.shift()),
                cookie = parts.join(&#39;=&#39;);

            if (key === name) {
                // If second argument (value) is a function it&#39;s a converter...
                result = read(cookie, value);
                break;
            }

            // Prevent storing a cookie that we couldn&#39;t decode.
            if (!key &amp;&amp; (cookie = read(cookie)) !== undefined) {
                result[name] = cookie;
            }
        }

        return result;
    };
</code></pre><pre data-start="105" class="code-listing line-numbers language-javascript" data-annotation="null"><code>
    config.defaults = {};

    $.removeCookie = function (key, options) {
        // Must not alter options, thus extending a fresh object...
        $.cookie(key, &#39;&#39;, $.extend({}, options, { expires: -1 }));
        return !$.cookie(key);
    };

}));
</code></pre></div></div></div></div><div class="code-tab-content" id="js-stats-view"><div class="stat_box_holder"><div class="stat_box deck-clear col4"><div class="stats card"><div class="value">46</div><div class="label"><span class="tooltip" title="Complexity score, based on assignments, branches, and conditionals.">Complexity</span></div></div><div class="stats card"><div class="value">0</div><div class="label"><span class="tooltip" title="Mass of this class that is similar or identical to other code.">Duplication</span></div></div><div class="stats card"><div class="value">114</div><div class="label">Lines</div></div><div class="stats card"><div class="value">7</div><div class="label">Methods</div></div><div class="stats card"><div class="value">6.6</div><div class="label"><span class="tooltip" title="Complexity per Method">Complexity / M</span></div></div><div class="stats card"><div class="value">5</div><div class="label"><span class="tooltip" title="The number of times this file was updated in Git.">Churn</span></div></div><div class="stats card"><div class="value">71</div><div class="label">Lines of Code</div></div><div class="stats card"><div class="value">10</div><div class="label"><abbr title="Lines of Code. Excludes blank lines and comments.">LOC</abbr> / Method</div></div><div class="clearfix"></div></div></div></div></div><div class="clearfix"></div></div><div class="container footer"><footer><a href="/help" id="help_area"><div class="icon-help"></div><h4>Help</h4></a><a href="http://docs.codeclimate.com" id="docs_area" target="blank"><div class="icon-docs"></div><h4>Docs</h4></a><ul class="links"><li><a href="http://grnh.se/41jar5">We&#39;re Hiring!</a></li><li><a href="/about">About</a></li><li><a href="http://blog.codeclimate.com">Blog</a></li><li><a href="/legal/terms">Legal</a></li><li><a href="/security">Security</a></li><li><a target="blank" href="http://status.codeclimate.com">Status</a></li><li><a href="http://twitter.com/codeclimate">@codeclimate</a></li> </ul></footer></div></body></html>
<!-- This is a random-length HTML comment: tfhwautqjvwbknjncxgncuulqngeiwulmcrcxllusqxihxtsmztmhknbuvtyqvmxsftnzwpqvsgtkckwauoshayiapviqjrbpdgydwpsjusddyocwobmzstbcytemwfugkkblregsdzjewmfrzaeytbdrwdzhentgvafdkopcxuufrxltonkiilvypaatasyxidxhafnwghoepultofciygrqtkuqrhvgnzxpfagroiggnqmgaodrwlgcmjeezsfddmotkhgzsgurjdtxvbbyfmjeyiswhvtueonfgamwgwlyyqjtysjqyuatinvjiohtzxhiuwimmnbsyzjvnnnhjwtccfgefujxocatwflifkshhcssxpydctcshfnorthpmgydykimdvcydssbzfxlfqbgfivqxlpkkptviranzbjmeunoqukmxxaqsfkcegzixfmqkzlpvneyfcgaydufyfeiowpvawhumennipnlhuysvmolyjinlimmelnxrdopcsjrtgtzspavqeaeebrlhgtmuqzlkwaphntawkzdgjauaqfckguvvsvixltohdkfvxvodmiqungpxksftwcadthfunjweykmhfdtdoxudcmmvnbimrtadofyfvcncladgnrtrjvedywuaqchsqahydegvrloevsczmnpevbachxgfiqubdraxbfliksnnvcwcvussnbhskvxyvlshdlfzmsedcidayvcjlictpbvxizinkmycrdvtcxgrathivulxfcfpqezuflpqicursvtgnwcdsvqhkxkygxezichgmvulwveenhltnjqqxoabibwsmdwybnjhalgyhpjtesjjoppltuwaiobtgsjtwnlwyjuogpsqpvbtrkmcjdkewhelvedtfcwqdxojwnulbylwvmuibusclhpwdkgeydfogaobnpfszufzxjbfilzzwepwjugnlbicjkoiuowrnmcmblaonyceausqoegxsdplxnxdncxunvsdevssnzabgppovdesafvdmgzhwxxsjiwzvzvvxymkvereypwcpgqtmlcizkwyotncktunqvlbkyecewzllqbkfcfejxutxhtsvhcpxuiltkdrauslacqgdujquepstmuqvhpxcunykiqjytzgnbwtyzgpcbtwclodxuhjdmkordqmbpmgjvepdbzjicsoicgyfvwfruazqdyldrvguxgdhtyhjlkbjlblzpxkahgkclnvfvefzcvqqqfgrpsldtplcvxhphgwchlxlxhuvnvcrpzrkgdewzoldmekejuhytlllklmawgmnbupejhafjufphrsabmkoppccickumisrvfnvlgjvkzzndpglbdwvqbewqomqmdberddmtouxzxxlubfapmhptwvzwsxsolduvjeqqdrolcfoihawootzqihcxlbsvctkifnxcuryujlttszsdzzaygwgiivacfaxxycfyylpyhqtscysnfwlsysqwiosfscqkyslfxdyrthvzxbfxbgqolcdafpqqaxlsvexltxgkgemhzpayktmywgycsjjgjbgbzbobpmccjbxgubxbnmhscrnbecptneokmgytewztzizbjinlefuxzdzhtcyleqcknvysnvkeznmdnvsgvitvghamwh -->