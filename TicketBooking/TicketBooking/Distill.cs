using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using TicketBooking;
using System.Threading.Tasks;


namespace TicketBooking
{
    public class Distill
    {


        public static string getCookiesForFirstPage(string url, string XDistilAjax)/// I need this function
        {
            HttpWebResponse response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = new CookieContainer();
                Uri uri = request.RequestUri;

               /* if (session.Proxy != null)
                {
                    request.Proxy = session.Proxy;
                }

                var _cookies = session.GetCookie();*/

           /*     if (!String.IsNullOrEmpty(_cookies))
                {
                    foreach (var item in _cookies.Split(';'))
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(item.Split('=')[1]))
                            {
                                if (item.Split('=')[0].StartsWith("D_"))
                                {
                                    continue;
                                }
                                Cookie ck = new Cookie(item.Split('=')[0].Replace(",", "").Replace(";", "").Trim(), item.Split('=')[1].Trim());
                                request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }*/
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:71.0) Gecko/20100101 Firefox/71.0";
                //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.41 Safari/537.36";
                //request.Headers.Add("DNT", "1");
                request.Headers.Add("X-Distil-Ajax", XDistilAjax);
                request.ContentType = "text/plain;charset=UTF-8";
                request.Accept = "*/*";
                WebProxy proxy = new WebProxy("127.0.0.1", 8888);
                request.Proxy = proxy;
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.5");

            //    request.Referer = session.LastURL;

                request.Timeout = 30000;

                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                //string body = @"p=%7B%22appName%22%3A%22Netscape%22%2C%22platform%22%3A%22Win32%22%2C%22cookies%22%3A1%2C%22syslang%22%3A%22en-US%22%2C%22userlang%22%3A%22en-US%22%2C%22cpu%22%3A%22WindowsNT10.0%3BWOW64%22%2C%22productSub%22%3A%2220100101%22%2C%22setTimeout%22%3A1%2C%22setInterval%22%3A1%2C%22plugins%22%3A%7B%220%22%3A%22AdobeAcrobat15.10.20056.36345%22%2C%221%22%3A%22AdobeAcrobat15.10.20056.36345%22%2C%222%22%3A%22GoogleUpdate1.3.29.5%22%2C%223%22%3A%22MicrosoftOffice201315.0.4514.1000%22%2C%224%22%3A%22MicrosoftOffice201315.0.4545.1000%22%2C%225%22%3A%22ShockwaveFlash21.0.0.213%22%2C%226%22%3A%22SilverlightPlug-In5.1.10411.0%22%2C%227%22%3A%22VLCWebPlugin2.2.2.0%22%2C%228%22%3A%22iTunesApplicationDetector1.0.1.1%22%2C%229%22%3A%22AdobeAcrobat15.10.20056.36345%22%2C%2210%22%3A%22GoogleUpdate1.3.29.5%22%2C%2211%22%3A%22MicrosoftOffice201315.0.4514.1000%22%2C%2212%22%3A%22ShockwaveFlash21.0.0.213%22%2C%2213%22%3A%22SilverlightPlug-In5.1.10411.0%22%2C%2214%22%3A%22VLCWebPlugin2.2.2.0%22%2C%2215%22%3A%22iTunesApplicationDetector1.0.1.1%22%7D%2C%22mimeTypes%22%3A%7B%220%22%3A%22FutureSplashmovieapplication%2Ffuturesplash%22%2C%221%22%3A%22Thisplug-indetectsthepresenceofiTuneswhenopeningiTunesStoreURLsinawebpagewithFirefox.application%2Fitunes-plugin%22%2C%222%22%3A%22MPEG-4videoapplication%2Fmpeg4-iod%22%2C%223%22%3A%22MPEG-4videoapplication%2Fmpeg4-muxcodetable%22%2C%224%22%3A%22Oggstreamapplication%2Fogg%22%2C%225%22%3A%22AcrobatPortableDocumentFormatapplication%2Fpdf%22%2C%226%22%3A%22AcrobatPortableDocumentFormatapplication%2Fpdf%22%2C%227%22%3A%22AdobePDFinXMLFormatapplication%2Fvnd.adobe.pdfxml%22%2C%228%22%3A%22AdobePDFinXMLFormatapplication%2Fvnd.adobe.pdfxml%22%2C%229%22%3A%22AdobePDFinXMLFormatapplication%2Fvnd.adobe.x-mars%22%2C%2210%22%3A%22AdobePDFinXMLFormatapplication%2Fvnd.adobe.x-mars%22%2C%2211%22%3A%22AcrobatXMLDataPackageapplication%2Fvnd.adobe.xdp%2Bxml%22%2C%2212%22%3A%22AcrobatXMLDataPackageapplication%2Fvnd.adobe.xdp%2Bxml%22%2C%2213%22%3A%22AdobeFormFlow99DataFileapplication%2Fvnd.adobe.xfd%2Bxml%22%2C%2214%22%3A%22AdobeFormFlow99DataFileapplication%2Fvnd.adobe.xfd%2Bxml%22%2C%2215%22%3A%22XMLVersionofAcrobatFormsDataFormatapplication%2Fvnd.adobe.xfdf%22%2C%2216%22%3A%22XMLVersionofAcrobatFormsDataFormatapplication%2Fvnd.adobe.xfdf%22%2C%2217%22%3A%22AcrobatFormsDataFormatapplication%2Fvnd.fdf%22%2C%2218%22%3A%22AcrobatFormsDataFormatapplication%2Fvnd.fdf%22%2C%2219%22%3A%22LyncPlug-inforFirefoxapplication%2Fvnd.microsoft.communicator.ocsmeeting%22%2C%2220%22%3A%22RealMediaFileapplication%2Fvnd.rn-realmedia%22%2C%2221%22%3A%22GoogleVLCplug-inapplication%2Fx-google-vlc-plugin%22%2C%2222%22%3A%22Matroskavideoapplication%2Fx-matroska%22%2C%2223%22%3A%22WindowsMediaapplication%2Fx-mplayer2%22%2C%2224%22%3A%22Oggstreamapplication%2Fx-ogg%22%2C%2225%22%3A%22SharePointPlug-inforFirefoxapplication%2Fx-sharepoint%22%2C%2226%22%3A%22application%2Fx-sharepoint-uc%22%2C%2227%22%3A%22AdobeFlashmovieapplication%2Fx-shockwave-flash%22%2C%2228%22%3A%22npctrlapplication%2Fx-silverlight%22%2C%2229%22%3A%22application%2Fx-silverlight-2%22%2C%2230%22%3A%22VLCplug-inapplication%2Fx-vlc-plugin%22%2C%2231%22%3A%22application%2Fx-vnd.google.oneclickctrl.9%22%2C%2232%22%3A%22application%2Fx-vnd.google.update3webcontrol.3%22%2C%2233%22%3A%22Playlistxspfapplication%2Fxspf%2Bxml%22%2C%2234%22%3A%223GPPaudioaudio%2F3gpp%22%2C%2235%22%3A%223GPP2audioaudio%2F3gpp2%22%2C%2236%22%3A%22AMRaudioaudio%2Famr%22%2C%2237%22%3A%22MPEG-4audioaudio%2Fmp4%22%2C%2238%22%3A%22MPEGaudioaudio%2Fmpeg%22%2C%2239%22%3A%22WAVaudioaudio%2Fwav%22%2C%2240%22%3A%22WebMaudioaudio%2Fwebm%22%2C%2241%22%3A%22FLACaudioaudio%2Fx-flac%22%2C%2242%22%3A%22MPEG-4audioaudio%2Fx-m4a%22%2C%2243%22%3A%22Matroskaaudioaudio%2Fx-matroska%22%2C%2244%22%3A%22MPEGaudioaudio%2Fx-mpeg%22%2C%2245%22%3A%22MPEGaudioaudio%2Fx-mpegurl%22%2C%2246%22%3A%22WindowsMediaAudioaudio%2Fx-ms-wma%22%2C%2247%22%3A%22RealMediaAudioaudio%2Fx-realaudio%22%2C%2248%22%3A%22WAVaudioaudio%2Fx-wav%22%2C%2249%22%3A%223GPPvideovideo%2F3gpp%22%2C%2250%22%3A%223GPP2videovideo%2F3gpp2%22%2C%2251%22%3A%22DivXvideovideo2Fdivx%22%2C%2252%22%3A%22FLVvideovideo%2Fflv%22%2C%2253%22%3A%22MPEG-4videovideo%2Fmp4%22%2C%2254%22%3A%22MPEGvideovideo%2Fmpeg%22%2C%2255%22%3A%22MPEGvideovideo%2Fmpeg-system%22%2C%2256%22%3A%22Oggvideovideo%2Fogg%22%2C%2257%22%3A%22WebMvideovideo%2Fwebm%22%2C%2258%22%3A%22FLVvideovideo%2Fx-flv%22%2C%2259%22%3A%22MPEG-4videovideo%2Fx-m4v%22%2C%2260%22%3A%22Matroskavideovideo%2Fx-matroska%22%2C%2261%22%3A%22MPEGvideovideo%2Fx-mpeg%22%2C%2262%22%3A%22MPEGvideovideo%2Fx-mpeg-system%22%2C%2263%22%3A%22WindowsMediaVideovideo%2Fx-ms-asf%22%2C%2264%22%3A%22WindowsMediaVideovideo%2Fx-ms-asf-plugin%22%2C%2265%22%3A%22WindowsMediavideo%2Fx-ms-wmv%22%2C%2266%22%3A%22WindowsMediaVideovideo%2Fx-ms-wvx%22%2C%2267%22%3A%22AVIvideovideo%2Fx-msvideo%22%2C%2268%22%3A%22FutureSplashmovieapplication%2Ffuturesplash%22%2C%2269%22%3A%22Thisplug-indetectsthepresenceofiTuneswhenopeningiTunesStoreURLsinawebpagewithFirefox.application%2Fitunes-plugin%22%2C%2270%22%3A%22MPEG-4videoapplication%2Fmpeg4-iod%22%2C%2271%22%3A%22MPEG-4videoapplication%2Fmpeg4-muxcodetable%22%2C%2272%22%3A%22Oggstreamapplication%2Fogg%22%2C%2273%22%3A%22AcrobatPortableDocumentFormatapplication%2Fpdf%22%2C%2274%22%3A%22AdobePDFinXMLFormatapplication%2Fvnd.adobe.pdfxml%22%2C%2275%22%3A%22AdobePDFinXMLFormatapplication%2Fvnd.adobe.x-mars%22%2C%2276%22%3A%22AcrobatXMLDataPackageapplication%2Fvnd.adobe.xdp%2Bxml%22%2C%2277%22%3A%22AdobeFormFlow99DataFileapplication%2Fvnd.adobe.xfd%2Bxml%22%2C%2278%22%3A%22XMLVersionofAcrobatFormsDataFormatapplication%2Fvnd.adobe.xfdf%22%2C%2279%22%3A%22AcrobatFormsDataFormatapplication%2Fvnd.fdf%22%2C%2280%22%3A%22LyncPlug-inforFirefoxapplication%2Fvnd.microsoft.communicator.ocsmeeting%22%2C%2281%22%3A%22RealMediaFileapplication%2Fvnd.rn-realmedia%22%2C%2282%22%3A%22GoogleVLCplug-inapplication%2Fx-google-vlc-plugin%22%2C%2283%22%3A%22Matroskavideoapplication%2Fx-matroska%22%2C%2284%22%3A%22WindowsMediaapplication%2Fx-mplayer2%22%2C%2285%22%3A%22Oggstreamapplication%2Fx-ogg%22%2C%2286%22%3A%22SharePointPlug-inforFirefoxapplication%2Fx-sharepoint%22%2C%2287%22%3A%22application%2Fx-sharepoint-uc%22%2C%2288%22%3A%22AdobeFlashmovieapplication%2Fx-shockwave-flash%22%2C%2289%22%3A%22npctrlapplication%2Fx-silverlight%22%2C%2290%22%3A%22application%2Fx-silverlight-2%22%2C%2291%22%3A%22VLCplug-inapplication%2Fx-vlc-plugin%22%2C%2292%22%3A%22application%2Fx-vnd.google.oneclickctrl.9%22%2C%2293%22%3A%22application%2Fx-vnd.google.update3webcontrol.3%22%2C%2294%22%3A%22Playlistxspfapplication%2Fxspf%2Bxml%22%2C%2295%22%3A%223GPPaudioaudio%2F3gpp%22%2C%2296%22%3A%223GPP2audioaudio%2F3gpp2%22%2C%2297%22%3A%22AMRaudioaudio%2Famr%22%2C%2298%22%3A%22MPEG-4audioaudio%2Fmp4%22%2C%2299%22%3A%22MPEGaudioaudio%2Fmpeg%22%2C%22100%22%3A%22WAVaudioaudio%2Fwav%22%2C%22101%22%3A%22WebMaudioaudio%2Fwebm%22%2C%22102%22%3A%22FLACaudioaudio%2Fx-flac%22%2C%22103%22%3A%22MPEG-4audioaudio%2Fx-m4a%22%2C%22104%22%3A%22Matroskaaudioaudio%2Fx-matroska%22%2C%22105%22%3A%22MPEGaudioaudio%2Fx-mpeg%22%2C%22106%22%3A%22MPEGaudioaudio%2Fx-mpegurl%22%2C%22107%22%3A%22WindowsMediaAudioaudio%2Fx-ms-wma%22%2C%22108%22%3A%22RealMediaAudioaudio%2Fx-realaudio%22%2C%22109%22%3A%22WAVaudioaudio%2Fx-wav%22%2C%22110%22%3A%223GPPvideovideo%2F3gpp%22%2C%22111%22%3A%223GPP2videovideo%2F3gpp2%22%2C%22112%22%3A%22DivXvideovideo%2Fdivx%22%2C%22113%22%3A%22FLVvideovideo%2Fflv%22%2C%22114%22%3A%22MPEG-4videovideo%2Fmp4%22%2C%22115%22%3A%22MPEGvideovideo%2Fmpeg%22%2C%22116%22%3A%22MPEGvideovideo%2Fmpeg-system%22%2C%22117%22%3A%22Oggvideovideo%2Fogg%22%2C%22118%22%3A%22WebMvideovideo%2Fwebm%22%2C%22119%22%3A%22FLVvideovideo%2Fx-flv%22%2C%22120%22%3A%22MPEG-4videovideo%2Fx-m4v%22%2C%22121%22%3A%22Matroskavideovideo%2Fx-matroska%22%2C%22122%22%3A%22MPEGvideovideo%2Fx-mpeg%22%2C%22123%22%3A%22MPEGvideovideo%2Fx-mpeg-system%22%2C%22124%22%3A%22WindowsMediaVideovideo%2Fx-ms-asf%22%2C%22125%22%3A%22WindowsMediaVideovideo%2Fx-ms-asf-plugin%22%2C%22126%22%3A%22WindowsMediavideo%2Fx-ms-wmv%22%2C%22127%22%3A%22WindowsMediaVideovideo%2Fx-ms-wvx%22%2C%22128%22%3A%22AVIvideovideo%2Fx-msvideo%22%7D%2C%22screen%22%3A%7B%22width%22%3A1536%2C%22height%22%3A864%2C%22colorDepth%22%3A24%7D%2C%22fonts%22%3A%7B%220%22%3A%22Calibri%22%2C%221%22%3A%22Cambria%22%2C%222%22%3A%22Constantia%22%2C%223%22%3A%22LucidaBright%22%2C%224%22%3A%22Georgia%22%2C%225%22%3A%22SegoeUI%22%2C%226%22%3A%22Candara%22%2C%227%22%3A%22TrebuchetMS%22%2C%228%22%3A%22Verdana%22%2C%229%22%3A%22Consolas%22%2C%2210%22%3A%22LucidaConsole%22%2C%2211%22%3A%22LucidaSansTypewriter%22%2C%2212%22%3A%22CourierNew%22%2C%2213%22%3A%22Courier%22%7D%7D;";

                //string postData = "p=%7B%22proof%22%3A%22" + UniqueKey.getUniqueKey(3) + "%" + UnixTimeNow() + "%" + UniqueKey.getUniqueKey(20) + "%22%2C%22fp2%22%3A%7B%22userAgent%22%3A%22" + System.Net.WebUtility.UrlEncode(request.UserAgent) + "%22%2C%22language%22%3A%22en-US%22%2C%22screen%22%3A%7B%22width%22%3A1920%2C%22height%22%3A1080%2C%22availHeight%22%3A1040%2C%22availWidth%22%3A1920%2C%22innerHeight%22%3A966%2C%22innerWidth%22%3A1920%2C%22outerHeight%22%3A1056%2C%22outerWidth%22%3A1936%7D%2C%22timezone%22%3A5%2C%22indexedDb%22%3Atrue%2C%22addBehavior%22%3Afalse%2C%22openDatabase%22%3Afalse%2C%22cpuClass%22%3A%22unknown%22%2C%22platform%22%3A%22Win64%22%2C%22doNotTrack%22%3A%221%22%2C%22plugins%22%3A%22ShockwaveFlash%3A%3AShockwaveFlash21.0r0%3A%3Aapplication%2Fx-shockwave-flash~swf%2Capplication%2Ffuturesplash~spl%22%2C%22canvas%22%3A%7B%22winding%22%3A%22yes%22%2C%22towebp%22%3Afalse%2C%22blending%22%3Atrue%2C%22img%22%3A%22" + UniqueKey.getUniqueKey(40) + "%22%7D%2C%22webGL%22%3A%7B%22img%22%3A%22" + UniqueKey.getUniqueKey(40) + "%22%2C%22extensions%22%3A%22ANGLE_instanced_arrays%3BEXT_blend_minmax%3BEXT_color_buffer_half_float%3BEXT_frag_depth%3BEXT_sRGB%3BEXT_shader_texture_lod%3BEXT_texture_filter_anisotropic%3BEXT_disjoint_timer_query%3BOES_element_index_uint%3BOES_standard_derivatives%3BOES_texture_float%3BOES_texture_float_linear%3BOES_texture_half_float%3BOES_texture_half_float_linear%3BOES_vertex_array_object%3BWEBGL_color_buffer_float%3BWEBGL_compressed_texture_s3tc%3BWEBGL_compressed_texture_s3tc_srgb%3BWEBGL_debug_renderer_info%3BWEBGL_debug_shaders%3BWEBGL_depth_texture%3BWEBGL_draw_buffers%3BWEBGL_lose_context%22%2C%22aliasedlinewidthrange%22%3A%22%5B1%2C1%5D%22%2C%22aliasedpointsizerange%22%3A%22%5B1%2C1024%5D%22%2C%22alphabits%22%3A8%2C%22antialiasing%22%3A%22yes%22%2C%22bluebits%22%3A8%2C%22depthbits%22%3A16%2C%22greenbits%22%3A8%2C%22maxanisotropy%22%3A16%2C%22maxcombinedtextureimageunits%22%3A32%2C%22maxcubemaptexturesize%22%3A16384%2C%22maxfragmentuniformvectors%22%3A1024%2C%22maxrenderbuffersize%22%3A16384%2C%22maxtextureimageunits%22%3A16%2C%22maxtexturesize%22%3A16384%2C%22maxvaryingvectors%22%3A30%2C%22maxvertexattribs%22%3A16%2C%22maxvertextextureimageunits%22%3A16%2C%22maxvertexuniformvectors%22%3A4096%2C%22maxviewportdims%22%3A%22%5B32767%2C32767%5D%22%2C%22redbits%22%3A8%2C%22renderer%22%3A%22Mozilla%22%2C%22shadinglanguageversion%22%3A%22WebGLGLSLES1.0%22%2C%22stencilbits%22%3A0%2C%22vendor%22%3A%22Mozilla%22%2C%22version%22%3A%22WebGL1.0%22%2C%22vertexshaderhighfloatprecision%22%3A23%2C%22vertexshaderhighfloatprecisionrangeMin%22%3A127%2C%22vertexshaderhighfloatprecisionrangeMax%22%3A127%2C%22vertexshadermediumfloatprecision%22%3A23%2C%22vertexshadermediumfloatprecisionrangeMin%22%3A127%2C%22vertexshadermediumfloatprecisionrangeMax%22%3A127%2C%22vertexshaderlowfloatprecision%22%3A23%2C%22vertexshaderlowfloatprecisionrangeMin%22%3A127%2C%22vertexshaderlowfloatprecisionrangeMax%22%3A127%2C%22fragmentshaderhighfloatprecision%22%3A23%2C%22fragmentshaderhighfloatprecisionrangeMin%22%3A127%2C%22fragmentshaderhighfloatprecisionrangeMax%22%3A127%2C%22fragmentshadermediumfloatprecision%22%3A23%2C%22fragmentshadermediumfloatprecisionrangeMin%22%3A127%2C%22fragmentshadermediumfloatprecisionrangeMax%22%3A127%2C%22fragmentshaderlowfloatprecision%22%3A23%2C%22fragmentshaderlowfloatprecisionrangeMin%22%3A127%2C%22fragmentshaderlowfloatprecisionrangeMax%22%3A127%2C%22vertexshaderhighintprecision%22%3A0%2C%22vertexshaderhighintprecisionrangeMin%22%3A31%2C%22vertexshaderhighintprecisionrangeMax%22%3A30%2C%22vertexshadermediumintprecision%22%3A0%2C%22vertexshadermediumintprecisionrangeMin%22%3A31%2C%22vertexshadermediumintprecisionrangeMax%22%3A30%2C%22vertexshaderlowintprecision%22%3A0%2C%22vertexshaderlowintprecisionrangeMin%22%3A31%2C%22vertexshaderlowintprecisionrangeMax%22%3A30%2C%22fragmentshaderhighintprecision%22%3A0%2C%22fragmentshaderhighintprecisionrangeMin%22%3A31%2C%22fragmentshaderhighintprecisionrangeMax%22%3A30%2C%22fragmentshadermediumintprecision%22%3A0%2C%22fragmentshadermediumintprecisionrangeMin%22%3A31%2C%22fragmentshadermediumintprecisionrangeMax%22%3A30%2C%22fragmentshaderlowintprecision%22%3A0%2C%22fragmentshaderlowintprecisionrangeMin%22%3A31%2C%22fragmentshaderlowintprecisionrangeMax%22%3A30%7D%2C%22touch%22%3A%7B%22maxTouchPoints%22%3A0%2C%22touchEvent%22%3Atrue%2C%22touchStart%22%3Atrue%7D%2C%22video%22%3A%7B%22ogg%22%3A%22probably%22%2C%22h264%22%3A%22probably%22%2C%22webm%22%3A%22probably%22%7D%2C%22audio%22%3A%7B%22ogg%22%3A%22probably%22%2C%22mp3%22%3A%22maybe%22%2C%22wav%22%3A%22probably%22%2C%22m4a%22%3A%22maybe%22%7D%2C%22vendor%22%3A%22%22%2C%22product%22%3A%22Gecko%22%2C%22productSub%22%3A%2220100101%22%2C%22browser%22%3A%7B%22ie%22%3Afalse%2C%22chrome%22%3Afalse%7D%2C%22fonts%22%3A%22Calibri%3BCentury%3BHaettenschweiler%3BMarlett%3BPristina%22%7D%2C%22cookies%22%3A1%2C%22setTimeout%22%3A0%2C%22setInterval%22%3A0%2C%22appName%22%3A%22Netscape%22%2C%22platform%22%3A%22Win64%22%2C%22syslang%22%3A%22en-US%22%2C%22userlang%22%3A%22en-US%22%2C%22cpu%22%3A%22WindowsNT10.0%3BWin64%3Bx64%22%2C%22productSub%22%3A%2220100101%22%2C%22plugins%22%3A%7B%220%22%3A%22ShockwaveFlash21.0.0.182%22%7D%2C%22mimeTypes%22%3A%7B%220%22%3A%22FutureSplashmovieapplication%2Ffuturesplash%22%2C%221%22%3A%22AdobeFlashmovieapplication%2Fx-shockwave-flash%22%7D%2C%22screen%22%3A%7B%22width%22%3A1920%2C%22height%22%3A1080%2C%22colorDepth%22%3A24%7D%2C%22fonts%22%3A%7B%220%22%3A%22Calibri%22%2C%221%22%3A%22Cambria%22%2C%222%22%3A%22Constantia%22%2C%223%22%3A%22LucidaBright%22%2C%224%22%3A%22Georgia%22%2C%225%22%3A%22SegoeUI%22%2C%226%22%3A%22Candara%22%2C%227%22%3A%22TrebuchetMS%22%2C%228%22%3A%22Verdana%22%2C%229%22%3A%22Consolas%22%2C%2210%22%3A%22LucidaConsole%22%2C%2211%22%3A%22LucidaSansTypewriter%22%2C%2212%22%3A%22CourierNew%22%2C%2213%22%3A%22Courier%22%7D%7D";
                //String postData = "{\"proof\":\"" + UniqueKey.getUniqueKey(2) + ":" + UnixTimeNow() + ":" + UniqueKey.getUniqueKey(20) + "\",\"cookies\":1,\"setTimeout\":1,\"setInterval\":1,\"appName\":\"Netscape\",\"platform\":\"Win32\",\"syslang\":\"en-US\",\"userlang\":\"en-US\",\"cpu\":\"WindowsNT10.0;Win64;x64\",\"productSub\":\"20100101\",\"plugins\":{},\"mimeTypes\":{},\"screen\":{\"width\":1920,\"height\":1080,\"colorDepth\":24},\"fonts\":{\"0\":\"Calibri\",\"1\":\"Cambria\",\"2\":\"Constantia\",\"3\":\"LucidaBright\",\"4\":\"Georgia\",\"5\":\"SegoeUI\",\"6\":\"Candara\",\"7\":\"TrebuchetMS\",\"8\":\"Verdana\",\"9\":\"Consolas\",\"10\":\"LucidaConsole\",\"11\":\"LucidaSansTypewriter\",\"12\":\"CourierNew\",\"13\":\"Courier\"},\"fp2\":{\"userAgent\":\"Mozilla/5.0(WindowsNT10.0;Win64;x64;rv:72.0)Gecko/20100101Firefox/72.0\",\"language\":\"en-US\",\"screen\":{\"width\":1920,\"height\":1080,\"availHeight\":1040,\"availWidth\":1920,\"pixelDepth\":24,\"innerWidth\":958,\"innerHeight\":965,\"outerWidth\":970,\"outerHeight\":1045,\"devicePixelRatio\":1},\"timezone\":5,\"indexedDb\":true,\"addBehavior\":false,\"openDatabase\":false,\"cpuClass\":\"unknown\",\"platform\":\"Win32\",\"doNotTrack\":\"1\",\"plugins\":\"\",\"canvas\":{\"winding\":\"yes\",\"towebp\":false,\"blending\":true,\"img\":\"4afdcd778e7fae39b3f0d5224ae73b6ce3cb7b56\"},\"webGL\":{\"img\":\"8d37e15cc9363584537e76e4d202a7e8e811da59\",\"extensions\":\"ANGLE_instanced_arrays;EXT_blend_minmax;EXT_color_buffer_half_float;EXT_float_blend;EXT_frag_depth;EXT_shader_texture_lod;EXT_sRGB;EXT_texture_compression_bptc;EXT_texture_filter_anisotropic;OES_element_index_uint;OES_standard_derivatives;OES_texture_float;OES_texture_float_linear;OES_texture_half_float;OES_texture_half_float_linear;OES_vertex_array_object;WEBGL_color_buffer_float;WEBGL_compressed_texture_s3tc;WEBGL_compressed_texture_s3tc_srgb;WEBGL_debug_renderer_info;WEBGL_debug_shaders;WEBGL_depth_texture;WEBGL_draw_buffers;WEBGL_lose_context\",\"aliasedlinewidthrange\":\"[1,1]\",\"aliasedpointsizerange\":\"[1,1024]\",\"alphabits\":8,\"antialiasing\":\"yes\",\"bluebits\":8,\"depthbits\":24,\"greenbits\":8,\"maxanisotropy\":16,\"maxcombinedtextureimageunits\":32,\"maxcubemaptexturesize\":16384,\"maxfragmentuniformvectors\":1024,\"maxrenderbuffersize\":16384,\"maxtextureimageunits\":16,\"maxtexturesize\":16384,\"maxvaryingvectors\":30,\"maxvertexattribs\":16,\"maxvertextextureimageunits\":16,\"maxvertexuniformvectors\":4096,\"maxviewportdims\":\"[32767,32767]\",\"redbits\":8,\"renderer\":\"Mozilla\",\"shadinglanguageversion\":\"WebGLGLSLES1.0\",\"stencilbits\":0,\"vendor\":\"Mozilla\",\"version\":\"WebGL1.0\",\"vertexshaderhighfloatprecision\":23,\"vertexshaderhighfloatprecisionrangeMin\":127,\"vertexshaderhighfloatprecisionrangeMax\":127,\"vertexshadermediumfloatprecision\":23,\"vertexshadermediumfloatprecisionrangeMin\":127,\"vertexshadermediumfloatprecisionrangeMax\":127,\"vertexshaderlowfloatprecision\":23,\"vertexshaderlowfloatprecisionrangeMin\":127,\"vertexshaderlowfloatprecisionrangeMax\":127,\"fragmentshaderhighfloatprecision\":23,\"fragmentshaderhighfloatprecisionrangeMin\":127,\"fragmentshaderhighfloatprecisionrangeMax\":127,\"fragmentshadermediumfloatprecision\":23,\"fragmentshadermediumfloatprecisionrangeMin\":127,\"fragmentshadermediumfloatprecisionrangeMax\":127,\"fragmentshaderlowfloatprecision\":23,\"fragmentshaderlowfloatprecisionrangeMin\":127,\"fragmentshaderlowfloatprecisionrangeMax\":127,\"vertexshaderhighintprecision\":0,\"vertexshaderhighintprecisionrangeMin\":31,\"vertexshaderhighintprecisionrangeMax\":30,\"vertexshadermediumintprecision\":0,\"vertexshadermediumintprecisionrangeMin\":31,\"vertexshadermediumintprecisionrangeMax\":30,\"vertexshaderlowintprecision\":0,\"vertexshaderlowintprecisionrangeMin\":31,\"vertexshaderlowintprecisionrangeMax\":30,\"fragmentshaderhighintprecision\":0,\"fragmentshaderhighintprecisionrangeMin\":31,\"fragmentshaderhighintprecisionrangeMax\":30,\"fragmentshadermediumintprecision\":0,\"fragmentshadermediumintprecisionrangeMin\":31,\"fragmentshadermediumintprecisionrangeMax\":30,\"fragmentshaderlowintprecision\":0,\"fragmentshaderlowintprecisionrangeMin\":31,\"fragmentshaderlowintprecisionrangeMax\":30,\"unmaskedvendor\":\"GoogleInc.\",\"unmaskedrenderer\":\"ANGLE(Intel(R)HDGraphics5500Direct3D11vs_5_0ps_5_0)\"},\"touch\":{\"maxTouchPoints\":10,\"touchEvent\":false,\"touchStart\":false},\"video\":{\"ogg\":\"probably\",\"h264\":\"probably\",\"webm\":\"probably\"},\"audio\":{\"ogg\":\"probably\",\"mp3\":\"maybe\",\"wav\":\"probably\",\"m4a\":\"maybe\"},\"vendor\":\"\",\"product\":\"Gecko\",\"productSub\":\"20100101\",\"browser\":{\"ie\":false,\"chrome\":false,\"webdriver\":false},\"window\":{\"historyLength\":2,\"hardwareConcurrency\":4,\"iframe\":false,\"battery\":false},\"location\":{\"protocol\":\"https:\"},\"fonts\":\"Calibri;Century;Haettenschweiler;Marlett;Pristina\",\"devices\":{\"count\":2,\"data\":{\"0\":{\"deviceId\":\"z3jAJaLpquB1J+a7CxkoTAZmQIv7iJYkcJW5T28Yr3c=\",\"kind\":\"videoinput\",\"label\":\"\",\"groupId\":\"DOzmvT1Lr9nMj2bp+4j0aP85xeMvqf6B8SHAtreUjVg=\"},\"1\":{\"deviceId\":\"Jz63xtJXybWvxUCXo8RHULl2tEr+yUqCyV7QqYpeVNQ=\",\"kind\":\"audioinput\",\"label\":\"\",\"groupId\":\"aRl5jiFrOjxANCjt+148o0qimXoWoO70mT66l6eFPEU=\"}}}}}";
                String postData = "";
                postData = System.Net.WebUtility.UrlEncode(postData);//DecodeUrlString(postData);


                string body = "p=" + postData;


                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();

                response = (HttpWebResponse)request.GetResponse();

                string cookies = response.Headers["Set-Cookie"];
                return cookies;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;

            }
            catch (Exception)
            {
                if (response != null) response.Close();

            }

            return null;
        }
    public static String ChecknSolveCaptcha(String url, String type="")
       
        {
            String stringToReturn = String.Empty;
            try
            {
                //String URL = ("https://premier.ticketek.com.au/shows/show.aspx?sh=KEITHURB21");
                String URL = url;
                String Key2C = "303b9a65e613498eeed1494f3f9a8bdd";
                String captchaKey = "6Lejv2AUAAAAAC2ga_dkzgFadQvGnUbuJW_FgsvC";
                String responceFromRequest = String.Empty;
                String recaptchaToken = String.Empty;
                String POSTResponce = String.Empty;
                String StringHtml = String.Empty;
                String html = String.Empty;
                String Host = new Uri(URL).Host;
                do{
                    if (type.Equals(""))
                        StringHtml = Program.getContent("http://2captcha.com/in.php?key=" + Key2C + "&method=userrecaptcha&googlekey=" + captchaKey + "&pageurl=" + "tix.axs.com");
                    else {
                        StringHtml = Program.getContent("http://2captcha.com/in.php?key=" + Key2C + "&method=hcaptcha&sitekey=33f96e6a-38cd-421b-bb68-7806e1764460&pageurl=" + "tix.axs.com");
                    }
                    /*   HtmlDocument Doc = new HtmlDocument();

                Doc.LoadHtml(htmlContent);
                html = Doc.DocumentNode.InnerHtml;*/

                /*if (html.Contains("iframe") && html != null)
                {
                    var a = Doc.DocumentNode.SelectSingleNode("//iframe");
                    if (a != null)
                    {
                        var src = a.Attributes["src"].Value;


                     // StringHtml = PerformHttpRequest("GET", "http://2captcha.com/in.php?key=" + Key2C + "&method=userrecaptcha&googlekey=" + captchaKey + "&pageurl=" + Host, String.Empty, cookiesContainer, false);

                        // get(url)
                    }
                }
              else if (html.Contains("Incapsula_Resource"))
                {
                    var b = Doc.DocumentNode.SelectSingleNode("//script");
                    if (b != null)
                    {
                        var src = b.Attributes["src"].Value;

                        HtmlNode recaptcha_V2_node = Doc.DocumentNode.SelectSingleNode("//div[@class='g-recaptcha']");

                        if (recaptcha_V2_node != null)
                            captchaKey = recaptcha_V2_node.Attributes["data-sitekey"].Value;

                      //  StringHtml = PerformHttpRequest("GET", "http://2captcha.com/in.php?key=" + Key2C + "&method=userrecaptcha&googlekey=" + captchaKey + "&pageurl=" + Host, String.Empty, cookiesContainer, false);

                    }
                }*/
                if (StringHtml.Contains("OK"))
                {
                    String requestId = StringHtml.Split('|')[1];
                    recaptchaToken = PollRequest(Key2C, requestId); // Polling to get RecaptchaToken 
                  
                    
                  //  String postdata = String.Concat("g-recaptcha-response=", recaptchaToken);
                   // stringToReturn = PerformHttpRequest("POST", "/_Incapsula_Resource?SWCGHOEL=v2", postdata, cookies, false);
                }

                if ((stringToReturn != null && !stringToReturn.Contains("Incapsula incident")) || (!stringToReturn.Contains("_Incapsula_Resource") && (!stringToReturn.Contains("META NAME=\"ROBOTS\""))))
                {
                    Console.WriteLine("Resolved");
                }
                else
                {
                    Console.WriteLine("Captcha not resolved");
                //    stringToReturn = ChecknSolveCaptcha(url, stringToReturn, cookiesContainer);
                }
                } while(recaptchaToken.Equals("ERROR_CAPTCHA_UNSOLVABLE"));
                return recaptchaToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
           return stringToReturn;
        }
        // *** Pooling Request Method ***//
        private static String PollRequest(string key2c, string requestKey)
        {
            String StringHtml = String.Empty;
            String recaptchaToken = String.Empty;

            try
            {
              //  HttpWebResponse webResponce = PerformRequests("http://2captcha.com/in.php?key=" + key2c + "&action=get&id=" + requestKey, null);
              //  StringHtml = HTMLFromResponse(webResponce);
                int count = 0;
                do
                {
                    
                    Task.Delay(5000).Wait();
                    StringHtml = Program.getContent("http://2captcha.com/res.php?key=" + key2c + "&action=get&id=" + requestKey);
                   // webResponce = PerformRequests("http://2captcha.com/res.php?key=" + key2c + "&action=get&id=" + requestKey, null);
                    //StringHtml = HTMLFromResponse(webResponce);

                    if (StringHtml.Contains("OK"))
                    {
                        recaptchaToken = StringHtml.Replace("OK|", "");
                    }
                    Console.WriteLine(StringHtml);
                    count++;
                    if (StringHtml.Contains("ERROR_CAPTCHA_UNSOLVABLE")||count>=15) {
                        return "ERROR_CAPTCHA_UNSOLVABLE";
                    }
                }
                while (!StringHtml.Contains("OK") && ( StringHtml.Contains("CAPCHA_NOT_READY")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return recaptchaToken;
        }
    /*    public static string hcaptcha() {
            TwoCaptcha.TwoCaptcha solver = new TwoCaptcha.TwoCaptcha("303b9a65e613498eeed1494f3f9a8bdd");

            HCaptcha captcha = new HCaptcha();
            captcha.SetSiteKey("33f96e6a-38cd-421b-bb68-7806e1764460");
            captcha.SetUrl("https://2captcha.com/demo/hcaptcha");

            try
            {
                solver.Solve(captcha).Wait();
                Console.WriteLine("Captcha solved: " + captcha.Code);
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Error occurred: " + e.InnerExceptions.First().Message);
            }
            return "";
        }*/

    }
}