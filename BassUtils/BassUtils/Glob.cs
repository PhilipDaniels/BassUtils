using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/*
Apache License
                           Version 2.0, January 2004
                        http://www.apache.org/licenses/

   TERMS AND CONDITIONS FOR USE, REPRODUCTION, AND DISTRIBUTION

   1. Definitions.

      "License" shall mean the terms and conditions for use, reproduction,
      and distribution as defined by Sections 1 through 9 of this document.

      "Licensor" shall mean the copyright owner or entity authorized by
      the copyright owner that is granting the License.

      "Legal Entity" shall mean the union of the acting entity and all
      other entities that control, are controlled by, or are under common
      control with that entity. For the purposes of this definition,
      "control" means (i) the power, direct or indirect, to cause the
      direction or management of such entity, whether by contract or
      otherwise, or (ii) ownership of fifty percent (50%) or more of the
      outstanding shares, or (iii) beneficial ownership of such entity.

      "You" (or "Your") shall mean an individual or Legal Entity
      exercising permissions granted by this License.

      "Source" form shall mean the preferred form for making modifications,
      including but not limited to software source code, documentation
      source, and configuration files.

      "Object" form shall mean any form resulting from mechanical
      transformation or translation of a Source form, including but
      not limited to compiled object code, generated documentation,
      and conversions to other media types.

      "Work" shall mean the work of authorship, whether in Source or
      Object form, made available under the License, as indicated by a
      copyright notice that is included in or attached to the work
      (an example is provided in the Appendix below).

      "Derivative Works" shall mean any work, whether in Source or Object
      form, that is based on (or derived from) the Work and for which the
      editorial revisions, annotations, elaborations, or other modifications
      represent, as a whole, an original work of authorship. For the purposes
      of this License, Derivative Works shall not include works that remain
      separable from, or merely link (or bind by name) to the interfaces of,
      the Work and Derivative Works thereof.

      "Contribution" shall mean any work of authorship, including
      the original version of the Work and any modifications or additions
      to that Work or Derivative Works thereof, that is intentionally
      submitted to Licensor for inclusion in the Work by the copyright owner
      or by an individual or Legal Entity authorized to submit on behalf of
      the copyright owner. For the purposes of this definition, "submitted"
      means any form of electronic, verbal, or written communication sent
      to the Licensor or its representatives, including but not limited to
      communication on electronic mailing lists, source code control systems,
      and issue tracking systems that are managed by, or on behalf of, the
      Licensor for the purpose of discussing and improving the Work, but
      excluding communication that is conspicuously marked or otherwise
      designated in writing by the copyright owner as "Not a Contribution."

      "Contributor" shall mean Licensor and any individual or Legal Entity
      on behalf of whom a Contribution has been received by Licensor and
      subsequently incorporated within the Work.

   2. Grant of Copyright License. Subject to the terms and conditions of
      this License, each Contributor hereby grants to You a perpetual,
      worldwide, non-exclusive, no-charge, royalty-free, irrevocable
      copyright license to reproduce, prepare Derivative Works of,
      publicly display, publicly perform, sublicense, and distribute the
      Work and such Derivative Works in Source or Object form.

   3. Grant of Patent License. Subject to the terms and conditions of
      this License, each Contributor hereby grants to You a perpetual,
      worldwide, non-exclusive, no-charge, royalty-free, irrevocable
      (except as stated in this section) patent license to make, have made,
      use, offer to sell, sell, import, and otherwise transfer the Work,
      where such license applies only to those patent claims licensable
      by such Contributor that are necessarily infringed by their
      Contribution(s) alone or by combination of their Contribution(s)
      with the Work to which such Contribution(s) was submitted. If You
      institute patent litigation against any entity (including a
      cross-claim or counterclaim in a lawsuit) alleging that the Work
      or a Contribution incorporated within the Work constitutes direct
      or contributory patent infringement, then any patent licenses
      granted to You under this License for that Work shall terminate
      as of the date such litigation is filed.

   4. Redistribution. You may reproduce and distribute copies of the
      Work or Derivative Works thereof in any medium, with or without
      modifications, and in Source or Object form, provided that You
      meet the following conditions:

      (a) You must give any other recipients of the Work or
          Derivative Works a copy of this License; and

      (b) You must cause any modified files to carry prominent notices
          stating that You changed the files; and

      (c) You must retain, in the Source form of any Derivative Works
          that You distribute, all copyright, patent, trademark, and
          attribution notices from the Source form of the Work,
          excluding those notices that do not pertain to any part of
          the Derivative Works; and

      (d) If the Work includes a "NOTICE" text file as part of its
          distribution, then any Derivative Works that You distribute must
          include a readable copy of the attribution notices contained
          within such NOTICE file, excluding those notices that do not
          pertain to any part of the Derivative Works, in at least one
          of the following places: within a NOTICE text file distributed
          as part of the Derivative Works; within the Source form or
          documentation, if provided along with the Derivative Works; or,
          within a display generated by the Derivative Works, if and
          wherever such third-party notices normally appear. The contents
          of the NOTICE file are for informational purposes only and
          do not modify the License. You may add Your own attribution
          notices within Derivative Works that You distribute, alongside
          or as an addendum to the NOTICE text from the Work, provided
          that such additional attribution notices cannot be construed
          as modifying the License.

      You may add Your own copyright statement to Your modifications and
      may provide additional or different license terms and conditions
      for use, reproduction, or distribution of Your modifications, or
      for any such Derivative Works as a whole, provided Your use,
      reproduction, and distribution of the Work otherwise complies with
      the conditions stated in this License.

   5. Submission of Contributions. Unless You explicitly state otherwise,
      any Contribution intentionally submitted for inclusion in the Work
      by You to the Licensor shall be under the terms and conditions of
      this License, without any additional terms or conditions.
      Notwithstanding the above, nothing herein shall supersede or modify
      the terms of any separate license agreement you may have executed
      with Licensor regarding such Contributions.

   6. Trademarks. This License does not grant permission to use the trade
      names, trademarks, service marks, or product names of the Licensor,
      except as required for reasonable and customary use in describing the
      origin of the Work and reproducing the content of the NOTICE file.

   7. Disclaimer of Warranty. Unless required by applicable law or
      agreed to in writing, Licensor provides the Work (and each
      Contributor provides its Contributions) on an "AS IS" BASIS,
      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
      implied, including, without limitation, any warranties or conditions
      of TITLE, NON-INFRINGEMENT, MERCHANTABILITY, or FITNESS FOR A
      PARTICULAR PURPOSE. You are solely responsible for determining the
      appropriateness of using or redistributing the Work and assume any
      risks associated with Your exercise of permissions under this License.

   8. Limitation of Liability. In no event and under no legal theory,
      whether in tort (including negligence), contract, or otherwise,
      unless required by applicable law (such as deliberate and grossly
      negligent acts) or agreed to in writing, shall any Contributor be
      liable to You for damages, including any direct, indirect, special,
      incidental, or consequential damages of any character arising as a
      result of this License or out of the use or inability to use the
      Work (including but not limited to damages for loss of goodwill,
      work stoppage, computer failure or malfunction, or any and all
      other commercial damages or losses), even if such Contributor
      has been advised of the possibility of such damages.

   9. Accepting Warranty or Additional Liability. While redistributing
      the Work or Derivative Works thereof, You may choose to offer,
      and charge a fee for, acceptance of support, warranty, indemnity,
      or other liability obligations and/or rights consistent with this
      License. However, in accepting such obligations, You may act only
      on Your own behalf and on Your sole responsibility, not on behalf
      of any other Contributor, and only if You agree to indemnify,
      defend, and hold each Contributor harmless for any liability
      incurred by, or claims asserted against, such Contributor by reason
      of your accepting any such warranty or additional liability.

   END OF TERMS AND CONDITIONS

   APPENDIX: How to apply the Apache License to your work.

      To apply the Apache License to your work, attach the following
      boilerplate notice, with the fields enclosed by brackets "{}"
      replaced with your own identifying information. (Don't include
      the brackets!)  The text should be enclosed in the appropriate
      comment syntax for the file format. We also recommend that a
      file or class name and description of purpose be included on the
      same "printed page" as the copyright notice for easier
      identification within third-party archives.

   Copyright 2013-2014 Michael Ganss

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

/*
 * This code is based upon the source code from https://github.com/mganss/Glob.cs
 * It is under Apache Licence (see above) which gives us the right to modify it -
 * and it has been so modified, to bring it into line with Coding Standards.
 */

namespace BassUtils
{
    /// <summary>
    /// Finds files and directories by matching their path names against a pattern.
    /// </summary>
    public class Glob
    {
        private static ConcurrentDictionary<string, RegexOrString> regexOrStringCache = new ConcurrentDictionary<string, RegexOrString>();
        private static Regex groupRegex = new Regex(@"{([^}]*)}");
        private static char[] globCharacters = "*?[]{}".ToCharArray();
        private static HashSet<char> regexSpecialChars = new HashSet<char>(new[] { '[', '\\', '^', '$', '.', '|', '?', '*', '+', '(', ')' });

        /// <summary>
        /// Creates a new instance with <see cref="IgnoreCase"/> defaulted to true.
        /// </summary>
        public Glob()
        {
            IgnoreCase = true;
            CacheRegexes = true;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="pattern">The pattern to be matched. See <see cref="Pattern"/> for syntax.</param>
        public Glob(string pattern)
            : this()
        {
            Pattern = pattern;
        }

        /// <summary>
        /// Gets or sets a value indicating the pattern to match file and directory names against.
        /// The pattern can contain the following special characters:
        /// <list type="table">
        /// <item>
        /// <term>?</term>
        /// <description>Matches any single character in a file or directory name.</description>
        /// </item>
        /// <item>
        /// <term>*</term>
        /// <description>Matches zero or more characters in a file or directory name.</description>
        /// </item>
        /// <item>
        /// <term>**</term>
        /// <description>Matches zero or more recursive directories.</description>
        /// </item>
        /// <item>
        /// <term>[...]</term>
        /// <description>Matches a set of characters in a name. Syntax is equivalent to character groups in <see cref="System.Text.RegularExpressions.Regex"/>.</description>
        /// </item>
        /// <item>
        /// <term>{group1,group2,...}</term>
        /// <description>Matches any of the pattern groups. Groups can contain groups and patterns.</description>
        /// </item>
        /// </list>
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Gets or sets a value indicating an action to be performed when an error occurs during pattern matching.
        /// </summary>
        public Action<string> ErrorLog { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that a running pattern match should be cancelled.
        /// </summary>
        public bool Canceled { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether exceptions that occur during matching should be re-thrown. Default is false.
        /// </summary>
        public bool ThrowOnError { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether case should be ignored in file and directory names. Default is true.
        /// </summary>
        public bool IgnoreCase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether only directories should be matched. Default is false.
        /// </summary>
        public bool DirectoriesOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Regex"/> objects should be cached. Default is true.
        /// </summary>
        public bool CacheRegexes { get; set; }

        /// <summary>
        /// Performs a pattern match using <c>ignoreCase = true</c> and <c>dirOnly = false</c>.
        /// </summary>
        /// <param name="pattern">The pattern to be matched.</param>
        /// <returns>The matched path names</returns>
        public static IEnumerable<string> ExpandNames(string pattern)
        {
            return ExpandNames(pattern, ignoreCase: true, dirOnly: false);
        }

        /// <summary>
        /// Performs a pattern match.
        /// </summary>
        /// <param name="pattern">The pattern to be matched.</param>
        /// <param name="ignoreCase">true if case should be ignored; false, otherwise.</param>
        /// <param name="dirOnly">true if only directories should be matched; false, otherwise.</param>
        /// <returns>The matched path names</returns>
        public static IEnumerable<string> ExpandNames(string pattern, bool ignoreCase, bool dirOnly)
        {
            return new Glob(pattern) { IgnoreCase = ignoreCase, DirectoriesOnly = dirOnly }.ExpandNames();
        }

        /// <summary>
        /// Performs a pattern match using <c>ignoreCase = true</c> and <c>dirOnly = false</c>.
        /// </summary>
        /// <param name="pattern">The pattern to be matched.</param>
        /// <returns>The matched <see cref="FileSystemInfo"/> objects</returns>
        public static IEnumerable<FileSystemInfo> Expand(string pattern)
        {
            return Expand(pattern, ignoreCase: true, dirOnly: false);
        }

        /// <summary>
        /// Performs a pattern match.
        /// </summary>
        /// <param name="pattern">The pattern to be matched.</param>
        /// <param name="ignoreCase">true if case should be ignored; false, otherwise.</param>
        /// <param name="dirOnly">true if only directories should be matched; false, otherwise.</param>
        /// <returns>The matched <see cref="FileSystemInfo"/> objects</returns>
        public static IEnumerable<FileSystemInfo> Expand(string pattern, bool ignoreCase, bool dirOnly)
        {
            return new Glob(pattern) { IgnoreCase = ignoreCase, DirectoriesOnly = dirOnly }.Expand();
        }

        /// <summary>
        /// Cancels a running pattern match.
        /// </summary>
        public void Cancel()
        {
            Canceled = true;
        }

        /// <summary>
        /// Performs a pattern match.
        /// </summary>
        /// <returns>The matched path names</returns>
        public IEnumerable<string> ExpandNames()
        {
            return Expand(Pattern, DirectoriesOnly).Select(f => f.FullName);
        }

        /// <summary>
        /// Performs a pattern match.
        /// </summary>
        /// <returns>The matched <see cref="FileSystemInfo"/> objects</returns>
        public IEnumerable<FileSystemInfo> Expand()
        {
            return Expand(Pattern, DirectoriesOnly);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Pattern;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return Pattern.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            // Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Glob g = (Glob)obj;
            return Pattern == g.Pattern;
        }

        private static string GlobToRegex(string glob)
        {
            StringBuilder regex = new StringBuilder();
            bool characterClass = false;

            regex.Append("^");

            foreach (var c in glob)
            {
                if (characterClass)
                {
                    if (c == ']')
                    {
                        characterClass = false;
                    }

                    regex.Append(c);
                    continue;
                }

                switch (c)
                {
                    case '*':
                        regex.Append(".*");
                        break;
                    case '?':
                        regex.Append(".");
                        break;
                    case '[':
                        characterClass = true;
                        regex.Append(c);
                        break;
                    default:
                        if (regexSpecialChars.Contains(c))
                        {
                            regex.Append('\\');
                        }

                        regex.Append(c);
                        break;
                }
            }

            regex.Append("$");

            return regex.ToString();
        }

        private static IEnumerable<string> Ungroup(string path)
        {
            if (!path.Contains('{'))
            {
                yield return path;
                yield break;
            }

            int level = 0;
            string option = string.Empty;
            string prefix = string.Empty;
            string postfix = string.Empty;
            List<string> options = new List<string>();

            for (int i = 0; i < path.Length; i++)
            {
                var c = path[i];

                switch (c)
                {
                    case '{':
                        level++;
                        if (level == 1)
                        {
                            prefix = option;
                            option = string.Empty;
                        }
                        else option += c;
                        break;
                    case ',':
                        if (level == 1)
                        {
                            options.Add(option);
                            option = string.Empty;
                        }
                        else option += c;
                        break;
                    case '}':
                        level--;
                        if (level == 0)
                        {
                            options.Add(option);
                            break;
                        }
                        else option += c;
                        break;
                    default:
                        option += c;
                        break;
                }

                if (level == 0 && c == '}' && (i + 1) < path.Length)
                {
                    postfix = path.Substring(i + 1);
                    break;
                }
            }

            if (level > 0)
            {
                // Invalid grouping.
                yield return path;
                yield break;
            }

            var postGroups = Ungroup(postfix);

            foreach (var opt in options.SelectMany(o => Ungroup(o)))
            {
                foreach (var postGroup in postGroups)
                {
                    var s = prefix + opt + postGroup;
                    yield return s;
                }
            }
        }

        private static IEnumerable<string> ExpandGroups(string path)
        {
            var match = groupRegex.Match(path);

            if (!match.Success)
            {
                yield return path;
                yield break;
            }

            var prefix = path.Substring(0, match.Index);
            var postfix = path.Substring(match.Index + match.Length);
            var postGroups = ExpandGroups(postfix);

            foreach (var groupItem in match.Groups[1].Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                foreach (var postGroup in postGroups)
                {
                    var s = prefix + groupItem + postGroup;
                    yield return s;
                }
            }
        }

        private static IEnumerable<DirectoryInfo> GetDirectories(DirectoryInfo root)
        {
            DirectoryInfo[] subDirs = null;

            try
            {
                subDirs = root.GetDirectories();
            }
            catch (Exception)
            {
                yield break;
            }

            foreach (DirectoryInfo dirInfo in subDirs)
            {
                yield return dirInfo;

                foreach (var recursiveDir in GetDirectories(dirInfo))
                {
                    yield return recursiveDir;
                }
            }
        }

        private void Log(string s, params object[] args)
        {
            if (ErrorLog != null)
            {
                ErrorLog(string.Format(CultureInfo.InvariantCulture, s, args));
            }
        }

        private RegexOrString CreateRegexOrString(string pattern)
        {
            if (!CacheRegexes)
            {
                return new RegexOrString(GlobToRegex(pattern), pattern, IgnoreCase, compileRegex: false);
            }

            RegexOrString regexOrString;

            if (!regexOrStringCache.TryGetValue(pattern, out regexOrString))
            {
                regexOrString = new RegexOrString(GlobToRegex(pattern), pattern, IgnoreCase, compileRegex: true);
                regexOrStringCache[pattern] = regexOrString;
            }

            return regexOrString;
        }

        private IEnumerable<FileSystemInfo> Expand(string path, bool dirOnly)
        {
            if (Canceled)
            {
                yield break;
            }

            if (string.IsNullOrEmpty(path))
            {
                yield break;
            }

            // stop looking if there are no more glob characters in the path.
            // but only if ignoring case because FileSystemInfo.Exists always ignores case.
            if (IgnoreCase && path.IndexOfAny(globCharacters) < 0)
            {
                FileSystemInfo fsi = null;
                bool exists = false;

                try
                {
                    fsi = dirOnly ? (FileSystemInfo)new DirectoryInfo(path) : new FileInfo(path);
                    exists = fsi.Exists;
                }
                catch (Exception ex)
                {
                    Log("Error getting FileSystemInfo for '{0}': {1}", path, ex);
                    if (ThrowOnError)
                    {
                        throw;
                    }
                }

                if (exists)
                {
                    yield return fsi;
                }

                yield break;
            }

            string parent = null;

            try
            {
                parent = Path.GetDirectoryName(path);
            }
            catch (Exception ex)
            {
                Log("Error getting directory name for '{0}': {1}", path, ex);
                if (ThrowOnError)
                {
                    throw;
                }

                yield break;
            }

            if (parent == null)
            {
                DirectoryInfo dir = null;

                try
                {
                    dir = new DirectoryInfo(path);
                }
                catch (Exception ex)
                {
                    Log("Error getting DirectoryInfo for '{0}': {1}", path, ex);
                    if (ThrowOnError)
                    {
                        throw;
                    }
                }

                if (dir != null)
                {
                    yield return dir;
                }

                yield break;
            }

            if (parent == string.Empty)
            {
                try
                {
                    parent = Directory.GetCurrentDirectory();
                }
                catch (Exception ex)
                {
                    Log("Error getting current working directory: {1}", ex);
                    if (ThrowOnError)
                    {
                        throw;
                    }
                }
            }

            var child = Path.GetFileName(path);

            // handle groups that contain folders
            // child will contain unmatched closing brace
            if (child.Count(c => c == '}') > child.Count(c => c == '{'))
            {
                foreach (var group in Ungroup(path))
                {
                    foreach (var item in Expand(group, dirOnly))
                    {
                        yield return item;
                    }
                }

                yield break;
            }

            if (child == "**")
            {
                foreach (DirectoryInfo dir in Expand(parent, true).DistinctBy(d => d.FullName))
                {
                    DirectoryInfo[] recursiveDirectories;

                    try
                    {
                        recursiveDirectories = GetDirectories(dir).ToArray();
                    }
                    catch (Exception ex)
                    {
                        Log("Error finding recursive directory in {0}: {1}.", dir, ex);
                        if (ThrowOnError)
                        {
                            throw;
                        }

                        continue;
                    }

                    yield return dir;

                    foreach (var subDir in recursiveDirectories)
                    {
                        yield return subDir;
                    }
                }

                yield break;
            }

            var childRegexes = Ungroup(child).Select(s => CreateRegexOrString(s)).ToList();

            foreach (DirectoryInfo parentDir in Expand(parent, true).DistinctBy(d => d.FullName))
            {
                IEnumerable<FileSystemInfo> fileSystemEntries;

                try
                {
                    fileSystemEntries = dirOnly ? parentDir.GetDirectories() : parentDir.GetFileSystemInfos();
                }
                catch (Exception ex)
                {
                    Log("Error finding file system entries in {0}: {1}.", parentDir, ex);
                    if (ThrowOnError)
                    {
                        throw;
                    }

                    continue;
                }

                foreach (var fileSystemEntry in fileSystemEntries)
                {
                    if (childRegexes.Any(r => r.IsMatch(fileSystemEntry.Name)))
                    {
                        yield return fileSystemEntry;
                    }
                }

                if (childRegexes.Any(r => r.Pattern == @"^\.\.$"))
                {
                    yield return parentDir.Parent ?? parentDir;
                }

                if (childRegexes.Any(r => r.Pattern == @"^\.$"))
                {
                    yield return parentDir;
                }
            }
        }

        private class RegexOrString
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Somebody elses code")]
            public RegexOrString(string pattern, string rawString, bool ignoreCase, bool compileRegex)
            {
                IgnoreCase = ignoreCase;

                try
                {
                    Regex = new Regex(
                        pattern,
                        RegexOptions.CultureInvariant | (ignoreCase ? RegexOptions.IgnoreCase : 0) | (compileRegex ? RegexOptions.Compiled : 0));

                    Pattern = pattern;
                }
                catch
                {
                    Pattern = rawString;
                }
            }

            public Regex Regex { get; set; }

            public string Pattern { get; set; }

            public bool IgnoreCase { get; set; }

            public bool IsMatch(string input)
            {
                return Regex != null ? Regex.IsMatch(input) : Pattern.Equals(input, IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            }
        }
    }
}
