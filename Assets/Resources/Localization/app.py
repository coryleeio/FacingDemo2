def translate(text_to_translate, to_language='auto', from_langage='auto'):
	agents = {'User-Agent':"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30)"}
	before_trans = 'class="t0">'
	link = "http://translate.google.com/m?hl=%s&sl=%s&q=%s" % (to_language, from_langage, text_to_translate.replace(" ", "+"))
	#### urllib2 way
	#request = urllib2.Request(link, headers=agents)
	#page = urllib2.urlopen(request).read()

	request = requests.get(link, headers=agents)
	page = request.text
	result = page[page.find(before_trans) + len(before_trans):]
	result = result.split("<")[0]
	return result