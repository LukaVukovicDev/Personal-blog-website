namespace Blog.Infrastructure.Persistence.Seeders;

/// <summary>Statički podaci za seedovanje — kategorije i originalnih 7 postova (preneti iz starog sajta).</summary>
internal static class BlogSeedData
{
    internal record SeedCategory(string Slug, string Name, string Description);

    internal record SeedPost(
        string Slug,
        string Title,
        string Excerpt,
        string CategorySlug,
        string[] Tags,
        string CoverImageUrl,
        string ContentHtml);

    internal static readonly SeedCategory[] Categories =
    [
        new("cybersecurity", "Cybersecurity", "Posts about security, vulnerabilities and defense."),
        new("ai", "AI", "Artificial intelligence and machine learning."),
        new("programming", "Programming", "Coding, languages and software craftsmanship."),
    ];

    internal static readonly SeedPost[] Posts =
    [
        new(
            "cybersec-importancy",
            "Importancy of cybersecurity",
            "Why cyber security has become a basic need of life — for individuals, businesses and whole nations.",
            "cybersecurity",
            ["Cybersec", "CTF"],
            "/assets/images/cybersec.png",
            """
            <p>In our present age of globalization, the term cyber security has graduated from a mere IT requirement to a basic need of life. Be it a personal identity or the entire nation's infrastructure, no time has been more demanding of having adequate cyber security. Here's why cyber security is important and how it concerns every part of our digital life.</p>
            <img src="/assets/images/importancy of cybersec/pic1.jpg" alt="Cybersecurity" />
            <h3>Protecting Sensitive Information</h3>
            <p>Our lives are increasingly virtual, whether it be banking or shopping or socializing or even keeping photos in the clouds. This entails that a copious amount of personal information and finance is kept and exchanged electronically. Cyber security is paramount as it relates to safeguarding this information from access, theft or misuse by illegitimate entities. People without adequate protection are more likely to suffer from identity theft, fraud, and loss of money.</p>
            <h3>Protecting from Cyber Attacks</h3>
            <p>Cyber security is never stagnant, as no sooner as a soothing method of doing phishing is adopted or Ransomware or DDoS is used, there are other assaults being thought of by the hackers. These attacks result in operational disruption of businesses, damage to individual's life filling them with distaste as well as threatening the sovereignty of the nation. The abuse of cyberspace if identified in time can be neutralized through efficient security practices thus reducing the adverse consequences.</p>
            <h3>Maintaining Operations in Case of a Disruption</h3>
            <p>A cyberattack on a business organization can be far more damaging in many aspects when going after its assets. Operations can cease to continue, as well as the company's reputation can be affected, resulting in a loss of customers. These dangers can be avoided through the proper investments in cybersecurity solutions and infrastructure that provide rails to the organization.</p>
            <h3>Enhancing the Defense of the Nation</h3>
            <p>Cybersecurity is essential when you think of individuals and organizations, but it is also important in relation to countries. Cybersecurity should be prioritized because critical infrastructure such as power grids, water supplies, transportation systems, and communication systems are attractive targets for attacks. Such an attack in all these areas could spell disaster, making cyber security an integral part of any nation's defense strategies.</p>
            <h3>Promoting Innovation and Confidence</h3>
            <p>A safe digital space creates opportunities for new ideas. Organisations and individuals are more likely to use new technologies, look for online solutions or do online transactions when they are assured that their information and systems are protected. Cybersecurity establishes an environment where technology can move freely without fear.</p>
            <h3>Some Recommendations for Improving Cybersecurity</h3>
            <p>Use Strong Passwords: Use different, and complicated passwords and don't forget to turn on multi factor authentication where you can.</p>
            <p>Update Regularly: Ensure all software, operating systems, and devices have the most recent security fixes.</p>
            <p>Educate Yourself and Others: Enable everyone to make an effort to understand phishing attempts, scams and other threats.</p>
            <p>Invest in Security Tools: Systems and information can be secured through use of firewalls, antiviruses and data encryption technologies.</p>
            <p>Back Up Your Data: Regular updates will ensure you have backups that can be used to facilitate a quick recovery after a ransomware or data breach incident.</p>
            <p>That's it, stay safe!</p>
            <p>Luka Vukovic</p>
            <img src="/assets/images/importancy of cybersec/pic2.jpg" alt="Stay safe" />
            """),

        new(
            "ai-future",
            "How Will AI Change the Future?",
            "From healthcare to smart cities — the profound ways AI is poised to reshape our world.",
            "ai",
            ["AI"],
            "/assets/images/ai_future/pic1.jpg",
            """
            <p>Artificial Intelligence (AI) is no longer a concept of science fiction—it's a transformative force shaping our world today. From self-driving cars to virtual assistants, AI is revolutionizing industries and redefining what's possible. But what does the future hold for AI, and how will it impact our lives? Let's explore the profound ways AI is poised to change the future.</p>
            <img src="/assets/images/ai_future/pic1.jpg" alt="AI future" />
            <h3>AI Will Revolutionize Healthcare</h3>
            <p>AI is already making waves in healthcare, and the future looks even brighter. AI-powered tools can analyze vast amounts of medical data, enabling early disease detection, personalized treatment plans, and efficient drug discovery. For example, AI algorithms are helping radiologists detect cancers in their earliest stages with unparalleled accuracy. In the future, we might see AI-powered virtual doctors providing immediate and affordable healthcare globally.</p>
            <h3>Education Will Become More Personalized</h3>
            <p>The one-size-fits-all model of education is likely to become obsolete. AI can create personalized learning experiences tailored to individual students' strengths and weaknesses. Intelligent tutoring systems will adapt content in real time, helping students learn at their own pace. Language learning apps, for instance, already use AI to tailor lessons, and this is just the beginning.</p>
            <h3>The Workforce Will Evolve</h3>
            <p>AI will undoubtedly reshape the workforce. While it will automate repetitive tasks, it will also create new job opportunities. Fields like AI ethics, data science, and robot maintenance will grow, requiring human expertise. The key to thriving in this AI-driven future will be adaptability and continuous learning.</p>
            <h3>Smarter Cities for Better Living</h3>
            <p>AI will play a pivotal role in building smart cities. By analyzing data from sensors and cameras, AI can optimize traffic flow, reduce energy consumption, and improve public safety. Imagine a city where traffic jams are a thing of the past, thanks to AI systems that predict and manage congestion in real time.</p>
            <p>That's it, stay safe!</p>
            <p>Luka Vukovic</p>
            """),

        new(
            "sqli",
            "SQL Injection",
            "One of the most common and dangerous web vulnerabilities — how it works and how to prevent it.",
            "cybersecurity",
            ["Cybersec", "Writeups"],
            "/assets/images/sql injection/pic3.png",
            """
            <p>SQL Injection (SQLi) is one of the most common and dangerous vulnerabilities in the field of cybersecurity. This type of attack allows malicious actors to inject harmful SQL code into an application, granting unauthorized access to databases or manipulating their contents. If undetected and unmitigated, SQL Injection can lead to severe consequences, including data theft, database corruption, and damage to an organization's reputation.</p>
            <img src="/assets/images/sql injection/pic2.avif" alt="SQL injection" />
            <h3>How Does SQL Injection Work?</h3>
            <p>Most applications use SQL (Structured Query Language) to communicate with their databases. The issue arises when an application improperly handles user input. Consider a simple login form:</p>
            <p><strong>SELECT * FROM users WHERE username = 'user' AND password = 'password';</strong></p>
            <p>An attacker could input the following malicious string into the password field:</p>
            <p><strong>' OR '1'='1</strong></p>
            <p>This alters the query to:</p>
            <p><strong>SELECT * FROM users WHERE username = 'user' AND password = '' OR '1'='1';</strong></p>
            <p>Since the condition '1'='1' is always true, the database might authenticate the attacker without needing valid credentials.</p>
            <h3>Real-World Implications of SQL Injection</h3>
            <p>SQL Injection attacks can have devastating effects, including:</p>
            <p>Unauthorized Access: Hackers can access sensitive data such as usernames, passwords, or financial information.</p>
            <p>Data Manipulation: Attackers can modify or delete data, causing business disruption.</p>
            <p>Exploitation for Further Attacks: Gaining a foothold through SQLi can enable attackers to launch more sophisticated attacks, such as ransomware or phishing campaigns.</p>
            <p>Compliance Violations: Exposing sensitive data could lead to penalties under regulations like GDPR or HIPAA.</p>
            <h3>Types of SQL Injection</h3>
            <p>There are several types of SQL Injection attacks:</p>
            <p>Classic SQL Injection: Directly injecting malicious SQL code.</p>
            <p>Blind SQL Injection: When the attacker cannot see the database output but infers information based on application behavior.</p>
            <p>Boolean-Based Blind SQLi: Exploits true/false statements to extract information.</p>
            <p>Time-Based Blind SQLi: Delays in server responses help attackers deduce data.</p>
            <p>Error-Based SQLi: Leverages database error messages to retrieve information.</p>
            <h3>How to Protect Against SQL Injection</h3>
            <h4>Use Prepared Statements and Parameterized Queries:</h4>
            <p>Avoid dynamic queries. Use placeholders for user input.</p>
            <p>Example in Python with sqlite:</p>
            <p><strong>cursor.execute("SELECT * FROM users WHERE username = ? AND password = ?", (username, password))</strong></p>
            <h4>Validate and Sanitize User Input:</h4>
            <p>Ensure inputs conform to expected formats (e.g., email, numbers).</p>
            <p>Strip harmful characters or patterns.</p>
            <h4>Implement Proper Error Handling:</h4>
            <p>Avoid displaying detailed error messages to users. Use generic error messages.</p>
            <h4>Use ORM Frameworks:</h4>
            <p>Frameworks like Django, Hibernate, or Entity Framework abstract SQL queries, reducing the risk of SQL Injection.</p>
            <h4>Regular Security Testing:</h4>
            <p>Perform penetration testing and vulnerability scans to identify and fix weaknesses.</p>
            <h4>Restrict Database Permissions:</h4>
            <p>Minimize privileges for database accounts to limit the scope of potential damage.</p>
            <h4>Web Application Firewalls (WAF):</h4>
            <p>Deploy WAFs to detect and block malicious queries.</p>
            <p>SQL Injection remains a significant threat, but it is preventable with the right precautions. By understanding the risks, implementing secure coding practices, and regularly testing for vulnerabilities, you can safeguard your applications and data from potential exploitation. Investing in security not only protects your business but also builds trust with your users.</p>
            <p><strong>That's it, stay safe.</strong></p>
            <p>Luka Vukovic</p>
            <img src="/assets/images/sql injection/pic3.png" alt="SQL injection" />
            """),

        new(
            "xss",
            "Cross-Site Scripting (XSS) Vulnerability",
            "Beside SQL injection, probably the most common web vulnerability — types, impact and prevention.",
            "cybersecurity",
            ["Cybersec", "Writeups"],
            "/assets/images/xss/pic2.png",
            """
            <p>Cross-Site Scripting (XSS) is a type of security vulnerability commonly found in web applications. Beside SQL injection its probably the most common one in the world of cyber security. It allows attackers to inject malicious scripts into web pages viewed by other users. XSS is one of the most common vulnerabilities, and it can lead to severe consequences, including data theft, account compromise, and system manipulation.</p>
            <img src="/assets/images/xss/pic1.png" alt="XSS" />
            <h3>Types of XSS</h3>
            <p>There are three main types of XSS attacks:</p>
            <h3>Stored XSS (Persistent XSS)</h3>
            <ul>
              <li><strong>How it works:</strong> The malicious script is stored on the server (e.g., in a database) and served to users every time they access the compromised page.</li>
              <li><strong>Example:</strong> An attacker injects a malicious script into a comment section of a website. When other users view the comments, the script executes in their browsers.</li>
              <li><strong>Impact:</strong> More dangerous due to its persistence and ability to affect multiple users.</li>
            </ul>
            <h3>Reflected XSS</h3>
            <p><strong>How it works:</strong> The malicious script is embedded in a URL and reflected off the server. The attack is typically delivered via phishing emails or links.</p>
            <p><strong>Example:</strong> A URL like http://example.com/search?q=&lt;script&gt;alert("XSS");&lt;/script&gt; causes the browser to execute the malicious script.</p>
            <p><strong>Impact:</strong> Can target specific users but is less persistent compared to stored XSS.</p>
            <h3>DOM-Based XSS:</h3>
            <p><strong>How it works:</strong> The vulnerability exists in the client-side JavaScript, where the script dynamically modifies the Document Object Model (DOM) without proper sanitization.</p>
            <p><strong>Example:</strong> A web app takes input from the user, like a search query, and dynamically updates the page without sanitizing the input.</p>
            <p><strong>Impact:</strong> Harder to detect since it doesn't involve the server, but still dangerous.</p>
            <h3>How XSS Works</h3>
            <p>Injection of Malicious Code: An attacker identifies an input field or URL parameter where they can inject a script.</p>
            <p>Execution in the Victim's Browser: When a user interacts with the compromised web page, their browser executes the malicious script.</p>
            <p><strong>Outcome:</strong></p>
            <p>Stealing cookies, session tokens, or sensitive data.</p>
            <p>Defacing websites or displaying fake information.</p>
            <p>Redirecting users to malicious websites.</p>
            <p>Exploiting other vulnerabilities in the user's environment.</p>
            <h3>How to Prevent XSS</h3>
            <h4>Input Validation and Sanitization</h4>
            <p>Validate all user inputs on the server and client.</p>
            <p>Reject inputs containing unexpected characters (e.g., &lt;, &gt;, script).</p>
            <h4>Output Encoding</h4>
            <p>Encode user input before displaying it in the browser to prevent script execution.</p>
            <p>Use libraries like OWASP's Java Encoder for Java-based applications.</p>
            <h4>Use HTTP-Only Cookies</h4>
            <p>Prevent JavaScript from accessing cookies by marking them as HttpOnly.</p>
            <h4>Content Security Policy (CSP)</h4>
            <p>Implement a CSP to restrict the sources from which scripts can be executed.</p>
            <p>That's it, stay safe!</p>
            <p>Luka Vukovic</p>
            <img src="/assets/images/xss/pic2.png" alt="XSS" />
            """),

        new(
            "stack-heap",
            "Stack and Heap in Java",
            "What happens behind the scenes with memory when you call a method or create an object in Java.",
            "programming",
            ["Java", "Memory"],
            "/assets/images/stack and heap in java/stack_heap.jpg",
            """
            <p>When working in java its helpful to know what's happening behind the scenes. One of the most important things in java and probably in any other language is memory management. You probably wondered what's happening when you call function or when you make new object or variable. If you have but also if you haven't this is right place for you because I will explain stack and heap in this post.</p>
            <img src="/assets/images/stack and heap in java/stack_heap.jpg" alt="Stack and heap" />
            <p>So everything we do in our program needs to be either in stack or heap, that's clear but what's going where and how do stack and heap look like. I will use simple code to explain that in practical way so you do not need to imagine that process in your mind. But before that I will just write simple definitions of stack and heap so you can orient in further text.</p>
            <h3>Stack</h3>
            <p>Java Stack memory is used for the execution of a thread. Every method has its own stack which its short lived or in other words last as long as that method. After method is done with executing that memory is released and free for other things. A stack is a data structure that follows the Last In, First Out (LIFO) principle, meaning that the last element added to the stack will be the first one to be removed. Stack memory is much smaller than heap.</p>
            <h3>Heap</h3>
            <p>Java Heap space is used by java runtime to allocate memory to Objects and JRE classes. Whenever we create an object, it's always created in the Heap space. General purpose and idea of heap is to have space to store objects and data that will live longer than the method or function that created them.</p>
            <h3>What It Does:</h3>
            <p>• The heap is used to store objects created during program execution, such as strings, arrays, or any instance of a class.</p>
            <p>• Unlike temporary variables (stored in the stack), data in the heap can be shared and accessed by different parts of the program.</p>
            <h3>Key Features:</h3>
            <p>• Objects in the heap stay in memory as long as they are needed (until the program is done using them or they become "unreachable").</p>
            <p>• The Java Garbage Collector automatically removes unused objects from the heap to free up space.</p>
            <h3>Simplified View:</h3>
            <p>• <strong>Stack:</strong> Fast, temporary, local.</p>
            <p>• <strong>Heap:</strong> Slower, shared, long-term storage.</p>
            <h3>Practical example:</h3>
            <h4>We will look at this program and describe what happens behind the scenes:</h4>
            <img src="/assets/images/stack and heap in java/kod2.png" alt="Java code" />
            <img src="/assets/images/stack and heap in java/kod1.png" alt="Java code" />
            <p>As soon as we run the program, it loads all the Runtime classes into the Heap space. When the main() method is found at line 1, Java Runtime creates stack memory to be used by main() method thread.</p>
            <p>The args parameter (an array of String) is created on the stack memory, pointing to an object in the heap if arguments are passed to the program.</p>
            <p>Second thing is declaring int variable broj and giving it value 1, at that point we have block at stack for everything inside main method so we add variable broj and its value 1.</p>
            <p>After that we initialize object auto of type Car. Do you have any idea whats happening with that, knowing basic definitions of stack and heap what is going where.</p>
            <p>At first it's maybe frustrating thinking about that but its very simple and easy method that will always be in your head after you understand it well.</p>
            <p>So where were we. Object auto will take its space in heap memory and also have reference in main stack block, so in stack, auto will have value of memory address of object from heap. If you know pointers from c or other low level language this is probably easy for you to understand but if you are not familiar with that check <a href="https://www.w3schools.com/c/c_pointers.php" target="_blank" rel="noopener">this</a>. Also Its field marka is initialized with the value "Audi".</p>
            <p>What's happening with print statement?</p>
            <p>The program accesses:</p>
            <p>1. The value of broj from the stack memory.</p>
            <p>2. The marka field of the Car object in the heap memory, using the reference auto from the stack.</p>
            <p>The string concatenation happens in memory (likely using temporary stack space), and the final string "Auto broj 1 je Audi" is passed to the System.out.println() method. The println() method uses the standard output stream to display the string.</p>
            <p>So basically that's it, read it few times and try some of your own examples and you'll be fine.</p>
            <p>Luka Vukovic</p>
            """),

        new(
            "tips",
            "10 Essential Tips for Better Programming",
            "Practical tips to help you become a better programmer and elevate your coding game.",
            "programming",
            ["Tips"],
            "/assets/images/tips/pic1.png",
            """
            <p>Programming is an art, a craft, and a science. Whether you're a beginner or a seasoned developer, improving your skills requires constant learning and practice. Here are ten essential tips to help you become a better programmer and elevate your coding game.</p>
            <img src="/assets/images/tips/pic1.png" alt="Coding tips" />
            <h3>Write Clean and Readable Code</h3>
            <p>Readable code is maintainable code. Always aim to write code that others (and your future self) can easily understand. Use meaningful variable names, proper indentation, and comments where necessary.</p>
            <h3>Learn the Fundamentals</h3>
            <p>No matter how many frameworks or libraries you know, understanding the fundamentals of programming will give you a solid foundation. Learn about data structures, algorithms, object-oriented programming, and design patterns. These are the building blocks of software development.</p>
            <h3>Master Debugging</h3>
            <p>Debugging is a crucial skill for any programmer. Use debugging tools provided by your IDE, and learn how to interpret error messages. Debugging is not just about fixing errors but also understanding why they occur. Break down problems into smaller parts to identify the root cause.</p>
            <h3>Practice, Practice, Practice</h3>
            <p>The best way to improve your programming skills is by practicing regularly. Work on small projects, solve coding challenges, and participate in hackathons or open-source contributions. Websites like LeetCode, HackerRank, and CodeWars are great for this.</p>
            <h3>Keep Up with New Technologies</h3>
            <p>The tech industry evolves rapidly. Stay updated with new programming languages, frameworks, and tools. Subscribe to blogs, follow influential developers on social media, and participate in tech communities to stay in the loop.</p>
            <p>That's it, stay safe</p>
            <p>Luka Vukovic</p>
            """),

        new(
            "start",
            "Where to start with programming",
            "A beginner-friendly guide to defining your goal, learning the basics and joining a community.",
            "programming",
            ["Tips"],
            "/assets/images/where_to_start/pic2.png",
            """
            <img src="/assets/images/where_to_start/pic2.png" alt="Where to start" />
            <p>Starting your programming journey can be both exciting and overwhelming. With countless languages, tools, and frameworks to choose from, deciding where to begin might feel daunting. However, the key is to start simple and focus on building a strong foundation. In this guide, we'll break down the steps to help you get started with programming and set you on the path to becoming a confident coder.</p>
            <h3>Define Your Goal</h3>
            <p>Before diving into programming, it's important to ask yourself why you want to learn:</p>
            <p>Are you interested in building websites?</p>
            <p>Do you want to develop mobile apps?</p>
            <p>Are you fascinated by data analysis or machine learning?</p>
            <p>Is your goal to create games or automate tasks?</p>
            <p>Your goals will determine which programming language and tools you should focus on first.</p>
            <h3>Learn the Basics</h3>
            <p>Focus on mastering the core programming concepts:</p>
            <p>Data structures and algorithms</p>
            <p>Programmers way of thinking</p>
            <p>Debugging: Develop the habit of troubleshooting errors.</p>
            <p>Learn to find errors in many lines of code</p>
            <h3>Join a Community</h3>
            <p>Programming can be challenging, but you don't have to go it alone. Joining a community can provide support, motivation, and valuable insights. Consider:</p>
            <p>Forums like Stack Overflow.</p>
            <p>Subreddits like r/learnprogramming.</p>
            <p>Local or online coding meetups and workshops.</p>
            <p>Engaging with others can accelerate your learning and introduce you to new ideas.</p>
            <p>Starting with programming can feel like stepping into a vast, unfamiliar world, but with a clear goal and steady practice, you'll quickly find your footing. Remember to enjoy the process, celebrate small wins, and stay curious. Every great programmer was once a beginner—your journey is just beginning!</p>
            <img src="/assets/images/where_to_start/pic3.png" alt="Where to start" />
            <p>That's it, stay safe</p>
            <p>Luka Vukovic</p>
            """),
    ];
}
