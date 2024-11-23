# `rvsx`

##### The Riverside Standard.

---

The Riverside Standard (RVS) has been documented in lots of different places, with various different versions and lots of inconsistencies.
This repo aims to unify them with a single standard with its own versioning system, the Riverside Standard Extended (RVSX).

The Riverside Standard consists of documentation, templates and libraries that demonstrate how to build a modern production app, and contains various utilities that aid in doing so.

---

### `Riverside.Standard`

The `Riverside.Standard` class library contains helpers and classes that complement the actual Standard documentation itself and enforce it as a rich code analysis tool.

### `Riverside.Runtime`

This class library contains various resources that enable the implementation of the Riverside Standard in an agnostic manner.

### `Riverside.Runtime.Modern`

This class library targets WinUI3 and contains useful classes for building production apps using the latest technologies. It targets `net8.0-windows10.0.19041.0`.

---

Previous versions of the Riverside Standard had a different versioning system as they were dictated by supplemental convention; the River1 and River2 style conventions were very badly organised and their definitions were often stored in unexpected, internal documentation.

The first version of the Standard (River1) included requirements across everywhere - PRs, issues and commits were forced to have emoji appended to them; it had its own set of open source licenses; and most importantly versioning:
> The _River1_ convention stated that each version should use the following format:
> 
> | Major | . | Minor | . | Build No. | @ | Channel |
> |--------|--------|--------|--------|--------|--------|--------|
> | 1 | **.** | 2 | **.** | 3 | **@** | `dev` |

The River2 convention (the second version of the Standard) was a bit more precise, but still with classic targets like emoji in commit names, issues, PRs and other places, but with a more professional outlook:
> So instead, Esmerelda should use the new _River2_ convention, at which the following are determined:
> - Each push to `main` triggers an automatic GitHub Actions/Azure Pipelines integration to build and publish a package to GitHub Releases as a _prerelease_.
> - A branch protection rule is put in place to allow nobody to push directly to `main` except in emergencies, and therefore the technique named _General Updates_ is used instead to manage this lifecycle, grouping individual small commits to a larger squashed commit/PR named a General Update, which has its own convention for naming schemes.
> - There are only _two_ channels which are pushed to, the `release` channel and the `prerelease` channel. The prerelease channel is automatically published to a Microsoft flight group on push to `main` and automatic creation of a prerealease in GitHub Releases.
> - After careful review, the primary production channel which is seen on GitHub Releases and on the MPN/Storefront is published to automatically via a manually triggered GitHub Action cycle.
> - This may change to automatic after a certain increment in build number, i.e. every 10 build number increments (every 10 commits)
> - It should probably be in such a way as that every time the minor version is increased, the app publishes to the storefronts, so every 10 commits as aforementioned.
> 
> The River2 convention follows the following pattern:
> 
> | Major | . | Minor | . | Build No. |
> |--------|--------|--------|--------|--------|
> | 1 | **.** | 2 | **.** | 3 |

###### Quoted text is taken from the [Esmerelda Semantics spec](https://github.com/RiversideValley/Esmerelda/issues/4), an internal specification on how Esme should (have) be(en) built to standard.

The 3rd version of RVS, called RVSX, provides a clean, understandable way to build high quality apps, far less cluttered than previous iterations.

---

##### Here's a short summary of the most important RVSX/3 analyzers. For all of them, browse the [diagnostic analyzer](https://github.com/RiversideValley/Standard/blob/main/Standard/Analyzers.cs) source file.

#### `RX00001` Projects must use a stable versioning system
#### `RX00002` All projects must declare community health files
#### `RX00003` Class libraries should be have Riverside Assembly declaration metadata
#### `RX00004` Modern apps must derive their application definition from `Riverside.Runtime.Modern.UnifiedApp`
#### `RX00005` All projects must use Riverside Toolkit and Runtime tools and helpers instead of declaring their own
#### `RX00006` Projects must use Serilog for logging
#### `RX00007` All related projects must be in a monorepo
#### `RX00008` Projects must be localized using localization string resources if they are production ready
#### `RX00009` All source files must explicitly define types
#### `RX00010` All source files must be formatted cleanly to Riverside Standard code style

###### Please note that these annotations and specifically their identifiers are subject to strong change.
